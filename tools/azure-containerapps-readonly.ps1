# Requires PowerShell 7+ and authenticated Azure CLI (az login).
# Only executes read-only "show" and "list" Azure CLI commands.
# No subscription/permission/resource modifications. Output remains local and private.
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$ResourceId,
    [string]$OutputPath = "artifacts/private/containerapps-evidence.json",
    [switch]$ValidateOnly
)

$ErrorActionPreference = 'Stop'
$resourcePattern = '^/subscriptions/(?<subscription>[0-9a-fA-F-]{36})/resourceGroups/(?<group>[A-Za-z0-9_().-]{1,90})/providers/Microsoft[.]App/containerApps/(?<app>[a-z0-9](?:[a-z0-9-]{0,30}[a-z0-9])?)$'
if ($ResourceId -notmatch $resourcePattern) {
    throw 'Expected one complete Microsoft.App/containerApps resource ID; refused all other scopes.'
}
$subGuid = [Guid]::Empty
if (-not [Guid]::TryParse($Matches.subscription, [ref]$subGuid)) {
    throw 'The subscription component must be a valid GUID.'
}
$appName = $Matches.app
$resourceGroup = $Matches.group
if ($ValidateOnly) {
    Write-Output 'READ_ONLY_TARGET_VALIDATED (offline: no Azure requests performed)'
    return
}

if (-not (Get-Command az -ErrorAction SilentlyContinue)) {
    throw 'Azure CLI was not found. Install Azure CLI and sign in with az login.'
}
# Do not let the CLI install a containerapp extension during a read-only probe.
$env:AZURE_EXTENSION_USE_DYNAMIC_INSTALL = 'no'

function Invoke-ReadOnlyAz {
    param([string[]]$Arguments)
    # Refuse any future accidental write-capable command if this script evolves.
    $prefix = $Arguments[0] + ' ' + $Arguments[1] + ' ' + $Arguments[2]
    if ($prefix -ne 'containerapp show --ids' -and
        $prefix -ne 'monitor metrics list') {
        throw 'Non-allowlisted Azure CLI operation blocked.'
    }
    $result = & az @Arguments 2>$null
    if ($LASTEXITCODE -ne 0) {
        throw 'A read-only Azure query failed: verify Azure login, scope and read permissions.'
    }
    return ($result | Out-String)
}

# Strict JMESPath projection: never request secrets or export raw Container App JSON.
# The CLI's --show-secrets flag is deliberately never used.
$projection = '{name:name,location:location,minReplicas:properties.template.scale.minReplicas,maxReplicas:properties.template.scale.maxReplicas,activeRevisionsMode:properties.configuration.activeRevisionsMode,latestRevision:properties.latestRevisionName}'
$configArgs = @('containerapp', 'show', '--ids', $ResourceId,
    '--query', $projection, '--output', 'json', '--only-show-errors')
$configJson = Invoke-ReadOnlyAz -Arguments $configArgs
$config = $configJson | ConvertFrom-Json
if (-not $config.name -or $config.name -ne $appName) {
    throw 'Resource identity mismatch or inaccessible Container App.'
}

$metricSpecs = @(
    @{ Name = 'Requests'; Aggregate = 'Total'; Result = 'sum' },
    @{ Name = 'Replicas'; Aggregate = 'Average'; Result = 'latest' },
    @{ Name = 'UsageNanoCores'; Aggregate = 'Average'; Result = 'mean' },
    @{ Name = 'WorkingSetBytes'; Aggregate = 'Average'; Result = 'mean' }
)
$metrics = @()
foreach ($spec in $metricSpecs) {
    $metric = [ordered]@{
        name = $spec.Name
        aggregation = $spec.Aggregate
        window = '24h'
        provenance = 'AZURE_MONITOR'
        availability = 'UNAVAILABLE'
        samples = 0
        observedValue = $null
    }
    try {
        $metricArgs = @('monitor','metrics','list','--resource',$ResourceId,
            '--metrics',$spec.Name,'--aggregation',$spec.Aggregate,
            '--offset','24h','--interval','PT1H',
            '--output','json','--only-show-errors')
        $body = (Invoke-ReadOnlyAz -Arguments $metricArgs) | ConvertFrom-Json
        $values = [System.Collections.Generic.List[double]]::new()
        foreach ($entry in @($body.value)) {
            if ($null -eq $entry) { continue }
            foreach ($series in @($entry.timeseries)) {
                if ($null -eq $series) { continue }
                foreach ($point in @($series.data)) {
                    if ($null -eq $point) { continue }
                    $candidate = if ($spec.Aggregate -eq 'Total') { $point.total } else { $point.average }
                    if ($null -ne $candidate) { $values.Add([double]$candidate) }
                }
            }
        }
        if ($values.Count -gt 0) {
            $metric.availability = 'OBSERVED'
            $metric.samples = $values.Count
            if ($spec.Result -eq 'sum') {
                $metric.observedValue = [Math]::Round(($values | Measure-Object -Sum).Sum, 4)
            } elseif ($spec.Result -eq 'latest') {
                $metric.observedValue = [Math]::Round($values[$values.Count - 1], 4)
            } else {
                $metric.observedValue = [Math]::Round(($values | Measure-Object -Average).Average, 4)
            }
        }
    } catch {
        # Missing permissions/unsupported metrics do not become invented zeroes.
        $metric.availability = 'UNAVAILABLE'
    }
    $metrics += [pscustomobject]$metric
}

$hashBytes = [Security.Cryptography.SHA256]::HashData(
    [Text.Encoding]::UTF8.GetBytes($ResourceId.ToLowerInvariant()))
$resourceHash = [Convert]::ToHexString($hashBytes).ToLowerInvariant()

$report = [ordered]@{
    schemaVersion = '1.0'
    mode = 'READ_ONLY_DIAGNOSTIC'
    observedAtUtc = [DateTimeOffset]::UtcNow.ToString('O')
    resourceIdSha256 = $resourceHash
    resourceName = $appName
    resourceGroup = $resourceGroup
    azureResource = 'Microsoft.App/containerApps'
    configuration = [ordered]@{
        minReplicas = $config.minReplicas
        maxReplicas = $config.maxReplicas
        activeRevisionsMode = $config.activeRevisionsMode
        latestRevision = $config.latestRevision
        location = $config.location
    }
    metrics = $metrics
    p95LatencyMs = $null
    p95Status = 'UNAVAILABLE_NOT_DERIVABLE_FROM_BASIC_CONTAINER_APPS_METRICS'
    actualAppCostUsd = $null
    costStatus = 'UNAVAILABLE_NOT_ATTRIBUTED_TO_SINGLE_APP'
    qualityGate = 'INCOMPLETE_EVIDENCE'
    humanApproval = 'NOT_REQUESTED'
    azureWritesEnabled = $false
    warning = 'Private metadata/metrics only; no p95 or attributable billing. Never infer PASS from this read-only snapshot.'
}
$absolutePath = [IO.Path]::GetFullPath($OutputPath)
$parent = Split-Path -Parent $absolutePath
New-Item -ItemType Directory -Path $parent -Force | Out-Null
# Ensure output isn't a credential or raw Azure config dump; omit resource ID.
$report | ConvertTo-Json -Depth 9 | Set-Content -Path $absolutePath -Encoding utf8
Write-Output "READ_ONLY_EVIDENCE_SAVED: $absolutePath"
Write-Output 'INCOMPLETE_EVIDENCE: p95 latency and per-app actual cost are not inferred.'

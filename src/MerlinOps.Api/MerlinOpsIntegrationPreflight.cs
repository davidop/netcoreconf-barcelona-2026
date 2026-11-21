namespace MerlinOps.Api;

/// <summary>
/// Safe, credential-free preflight. This NEVER contacts Azure/Copilot and never
/// claims that an endpoint, model or tenant is reachable or authorized.
/// </summary>
public static class MerlinOpsIntegrationPreflight
{
    public static IntegrationReadiness Inspect(
        string? foundryProjectEndpoint, string? primaryModel, string? fallbackModel)
    {
        var endpointSafe = Uri.TryCreate(foundryProjectEndpoint, UriKind.Absolute, out var uri)
            && uri.Scheme == Uri.UriSchemeHttps
            && uri.Host.EndsWith(".services.ai.azure.com", StringComparison.OrdinalIgnoreCase)
            && uri.AbsolutePath.Contains("/api/projects/", StringComparison.OrdinalIgnoreCase)
            && uri.UserInfo.Length == 0
            && uri.Query.Length == 0
            && uri.Fragment.Length == 0;
        bool distinctModels = !string.IsNullOrWhiteSpace(primaryModel)
            && !string.IsNullOrWhiteSpace(fallbackModel)
            && !string.Equals(primaryModel, fallbackModel, StringComparison.OrdinalIgnoreCase);
        return new IntegrationReadiness(
            CopilotStudio: "NOT_CONNECTED",
            PreferredProtocol: "A2A_PENDING_VALIDATION",
            FallbackProtocol: "AUTHENTICATED_REST_PENDING_VALIDATION",
            Maf: "REAL_LOCAL_DETERMINISTIC_EXECUTORS",
            Foundry: "NOT_CONNECTED",
            FoundryTargetProject: "ai-omniroute-dev",
            FoundryEndpointShapeValidated: endpointSafe,
            ModelPairSpecified: distinctModels,
            ModelUsageProvenance: "UNAVAILABLE_UNTIL_VERIFIED_INFERENCE",
            Presentation: "LOCAL_SIMULATED_AND_PUBLIC_REPLAY",
            CloudWritesEnabled: false);
    }

    public static IntegrationReadiness InspectEnvironment() =>
        Inspect(
            Environment.GetEnvironmentVariable("MERLINOPS_FOUNDRY_PROJECT_ENDPOINT"),
            Environment.GetEnvironmentVariable("MERLINOPS_FOUNDRY_PRIMARY_MODEL"),
            Environment.GetEnvironmentVariable("MERLINOPS_FOUNDRY_FALLBACK_MODEL"));
}

/// <summary>Never includes endpoints, credentials, tenant IDs, model secrets or claims of connection.</summary>
public sealed record IntegrationReadiness(
    string CopilotStudio, string PreferredProtocol, string FallbackProtocol,
    string Maf, string Foundry, string FoundryTargetProject,
    bool FoundryEndpointShapeValidated, bool ModelPairSpecified,
    string ModelUsageProvenance, string Presentation, bool CloudWritesEnabled);

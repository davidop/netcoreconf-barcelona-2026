using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Agents.AI.Workflows;

namespace MerlinOps.Api;

/// <summary>
/// The agreed conference mission: MerlinOps improves its OWN incident resolution workflow.
/// Executes real local .NET tool adapters and a real MAF fan-out; evidence is synthetic.
/// Foundry and Copilot Studio are NOT connected and are never implicitly invoked.
/// </summary>
public sealed class IncidentImprovementEngine(PolicyEngine policy)
{
    public static readonly string[] Cases =
        ["incident-improvement", "incident-policy-block", "incident-regression", "incident-missing-evidence"];

    public async Task<IncidentImprovementReport> CompareAsync(
        string caseId, string? sandboxDirectory = null, CancellationToken cancellationToken = default)
    {
        if (!Cases.Contains(caseId, StringComparer.Ordinal))
            throw new ArgumentException("Unknown incident scenario", nameof(caseId));

        var fixture = new SyntheticIncident(
            "INC-2042", "order-processing", "Intermittent 503 and repeated dependency checks",
            "trace-order-2042", 12.0, 4, 0.82);
        if (caseId == "incident-missing-evidence")
            fixture = fixture with { CorrelationTrace = null };

        var snapshot = JsonSerializer.Serialize(fixture);
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(snapshot))).ToLowerInvariant();
        var audit = new List<IncidentAuditEvent>();
        void Emit(string kind, string message, string gate = "NONE") =>
            audit.Add(new(audit.Count + 1, DateTimeOffset.UtcNow, kind, message, gate, "SIMULATED"));

        Emit("incident.received", "Local synthetic incident fixture received; cloud access and writes disabled.");
        var v1 = await RunVersionAsync("v1", fixture, cancellationToken);
        Emit("workflow.v1.completed", $"v1: {v1.ToolCalls} local diagnostic tool calls and four real MAF Guardian completions.");
        var v2 = await RunVersionAsync("v2", fixture, cancellationToken);
        Emit("workflow.v2.completed", $"v2: {v2.ToolCalls} local diagnostic tool calls and four real MAF Guardian completions.");

        var callReduction = Math.Round(100.0 * (v1.ToolCalls - v2.ToolCalls) / v1.ToolCalls, 1);
        var quality = Math.Min(v1.QualityScore, v2.QualityScore);
        var retriesProposed = caseId == "incident-policy-block" ? 8 : 2;
        var proposal = caseId == "incident-policy-block"
            ? "Unsafe: raise automated incident retries to 8, exceeding the safe retry budget."
            : "Replace duplicated diagnostic queries with a structured collection of incident, dependency, trace and policy evidence; keep retry limit at 2.";

        // The model or Guardian can suggest a change; only deterministic backend logic authorizes review.
        GuardDecision gate;
        if (fixture.CorrelationTrace is null)
            gate = new(false, "MISSING_REQUIRED_TRACE");
        else if (quality < 0.90)
            gate = new(false, "QUALITY_BELOW_90_PERCENT");
        else if (callReduction < 20.0)
            gate = new(false, "TOOL_CALL_OPTIMIZATION_NOT_MET");
        else
            gate = policy.EvaluateIncidentWorkflow(quality, retriesProposed);

        var status = gate.Allowed ? "AWAITING_HUMAN_REVIEW" : "BLOCKED";
        LocalRollbackProof? rollback = null;
        Emit("evaluation.completed",
            $"Deterministic rubric: {quality:P0}; calls: {v1.ToolCalls} → {v2.ToolCalls} ({callReduction:F1}% saved). Actual local durations measured; not Azure latency.",
            gate.Allowed ? "PASS" : "DENY");
        Emit("policy.evaluated", gate.Reason, gate.Allowed ? "PASS" : "DENY");

        if (!gate.Allowed)
            Emit("mission.blocked", "No approval request, workflow publication or cloud change.", "DENY");
        else if (caseId == "incident-regression")
        {
            Emit("quality.regression", "Synthetic post-improvement quality regression: 0.73. Restore the LOCAL incident workflow manifest.", "DENY");
            var dir = sandboxDirectory ?? Path.Combine(Path.GetTempPath(), "merlinops-incident-" + Guid.NewGuid().ToString("N"));
            rollback = RestoreIncidentWorkflowV1(dir);
            status = rollback.Verified ? "ROLLED_BACK_LOCAL" : "RECOVERY_FAILED";
            Emit(rollback.Verified ? "rollback.verified" : "rollback.failed",
                rollback.Verified ? "Local active incident workflow is v1; SHA-256 integrity verified." : "Local restoration failed verification.",
                rollback.Verified ? "PASS" : "DENY");
        }
        else
            Emit("approval.required",
                "Technical PASS only: Érika must review evidence. Human approval is NOT_REQUESTED; nothing is deployed.", "PENDING");

        return new IncidentImprovementReport(
            Guid.NewGuid().ToString("N"), caseId, "SIMULATED", "SYNTHETIC_FIXTURE",
            "Copilot Studio → MAF → Foundry → Quality Gates → Slidev (TARGET ONLY: cloud integrations not connected)",
            fixture.IncidentId, fixture.Service, hash,
            "Resolve INC-2042 with fewer duplicate diagnostic tool calls while preserving quality, policy and auditability.",
            v1, v2, callReduction, proposal,
            gate.Allowed ? "PASS" : "DENY", gate.Reason, status, "NOT_REQUESTED",
            new ModelInferenceEvidence("Microsoft Foundry", "ai-omniroute-dev", "NOT_CONNECTED",
                null, null, null, null, "NO_MODEL_CALL_PERFORMED"),
            false, rollback, audit);
    }

    private static async Task<IncidentWorkflowVersion> RunVersionAsync(
        string version, SyntheticIncident fixture, CancellationToken ct)
    {
        var invocations = new List<string>();
        var sw = Stopwatch.StartNew();
        async Task Call(string operation)
        {
            ct.ThrowIfCancellationRequested();
            // Deterministic synthetic adapter: 4 ms transport simulation, NOT Azure latency.
            await Task.Delay(4, ct);
            invocations.Add(operation);
        }

        if (version == "v1")
        {
            await Call("incident.fetch");
            await Call("dependency.fetch");
            await Call("trace.fetch");
            await Call("incident.fetch");
            await Call("dependency.fetch");
            await Call("policy.fetch");
            await Call("trace.fetch");
            await Call("incident.fetch");
            await Call("policy.fetch");
        }
        else
        {
            await Call("incident.fetch");
            await Call("dependency.fetch");
            await Call("trace.fetch");
            await Call("policy.fetch");
        }

        // Actually execute Microsoft Agent Framework, not a pre-authored animation.
        Func<string, string> planner = input => "Incident workflow planned for " + input;
        Func<string, string> telemetry = _ => "Incident trace: " + (fixture.CorrelationTrace ?? "MISSING");
        Func<string, string> finops = _ => "Local diagnostic tool requests counted, remote token/cost usage unavailable";
        Func<string, string> quality = _ => "Evidence completeness checked with deterministic rubric";
        Func<string, string> governance = _ => "Independent backend policy is authoritative; approval not granted";

        var orchestrator = planner.BindAsExecutor("IncidentPlanner");
        var guardians = new[]
        {
            telemetry.BindAsExecutor("TelemetryGuardian"),
            finops.BindAsExecutor("FinOpsGuardian"),
            quality.BindAsExecutor("QualityGuardian"),
            governance.BindAsExecutor("GovernanceGuardian")
        };
        var builder = new WorkflowBuilder(orchestrator);
        builder.AddFanOutEdge(orchestrator, guardians).WithOutputFrom(guardians);
        var workflow = builder.Build();
        await using Run run = await InProcessExecution.RunAsync(workflow, version + ":" + fixture.IncidentId);
        var completed = new HashSet<string>(StringComparer.Ordinal);
        foreach (WorkflowEvent evt in run.NewEvents)
            if (evt is ExecutorCompletedEvent eventCompleted &&
                eventCompleted.ExecutorId.EndsWith("Guardian", StringComparison.Ordinal) &&
                !completed.Add(eventCompleted.ExecutorId))
                throw new InvalidOperationException("Duplicate MAF Guardian completion: fail closed.");
        if (completed.Count != 4)
            throw new InvalidOperationException("MAF did not finish all required Guardian checks: fail closed.");

        sw.Stop();
        // Quality is synthetic evidence-completeness rubric, NOT an LLM judge.
        double completeness = fixture.CorrelationTrace is null ? 0.75 : 1.0;
        return new IncidentWorkflowVersion(version, invocations.Count,
            Math.Round(sw.Elapsed.TotalMilliseconds, 2), "LOCAL_MEASURED_SYNTHETIC_TOOLS",
            completeness, completed.Count, invocations.ToArray());
    }

    private static LocalRollbackProof RestoreIncidentWorkflowV1(string directory)
    {
        Directory.CreateDirectory(directory);
        var original = Path.Combine(directory, "incident-v1.json");
        var improved = Path.Combine(directory, "incident-v2.json");
        var active = Path.Combine(directory, "incident-active.json");

        const string version1 = "{\"version\":\"v1\",\"workflow\":\"incident-resolution\",\"toolCalls\":9}";
        const string version2 = "{\"version\":\"v2\",\"workflow\":\"incident-resolution\",\"toolCalls\":4}";
        File.WriteAllText(original, version1);
        File.WriteAllText(improved, version2);
        File.Copy(improved, active, true);
        if (File.ReadAllText(active) != version2)
            throw new InvalidOperationException("Local v2 activation could not be verified.");
        File.Copy(original, active, true);
        string Sha(string p) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
        var expected = Sha(original);
        var actual = Sha(active);
        return new LocalRollbackProof("v1", expected, actual,
            expected == actual && File.ReadAllText(active) == version1);
    }

    private sealed record SyntheticIncident(
        string IncidentId, string Service, string Symptom, string? CorrelationTrace,
        double ErrorRatePercent, int DependencyFailures, double BaselineP95Seconds);
}

public sealed record IncidentWorkflowVersion(string Version, int ToolCalls, double ObservedLocalDurationMs,
    string DurationProvenance, double QualityScore, int MafGuardiansCompleted, IReadOnlyList<string> ToolTrace);
public sealed record ModelInferenceEvidence(string Provider, string PlannedProject, string Status, string? Model,
    int? InputTokens, int? OutputTokens, decimal? CostUsd, string UsageProvenance);
public sealed record IncidentAuditEvent(int Sequence, DateTimeOffset OccurredAt, string Kind,
    string Message, string Gate, string Provenance);
public sealed record IncidentImprovementReport(
    string Id, string CaseId, string Mode, string EvidenceSource, string IntendedArchitecture,
    string IncidentId, string Service, string EvidenceSha256, string UserRequest,
    IncidentWorkflowVersion V1, IncidentWorkflowVersion V2, double ToolCallReductionPercent,
    string ProposedChange, string TechnicalDecision, string DecisionReason, string Outcome,
    string HumanApproval, ModelInferenceEvidence Foundry, bool CloudWritesEnabled,
    LocalRollbackProof? Rollback, IReadOnlyList<IncidentAuditEvent> Audit);

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MerlinOps.Api;

/// <summary>
/// Erika's evidence-first vertical slice: a REAL local tool-call comparison,
/// using SYNTHETIC Azure Container Apps evidence. No Azure/model/Teams calls.
/// </summary>
public sealed class ErikaOptimizationEngine(PolicyEngine policy)
{
    public static readonly string[] Cases = ["finops-success", "policy-block", "quality-rollback", "missing-metrics"];

    public ErikaComparison Compare(string caseId, string? rollbackDirectory = null)
    {
        if (!Cases.Contains(caseId, StringComparer.Ordinal))
            throw new ArgumentException("Unknown case", nameof(caseId));

        // Identical immutable snapshot is used by both variants.
        var evidence = new AppEvidence(
            Resource: "/subscriptions/fictional/resourceGroups/demo/providers/Microsoft.App/containerApps/merlinops-api",
            MinReplicas: 3, MaxReplicas: 12, HttpConcurrency: 50,
            Requests24h: 16000, P95LatencyMs: caseId == "missing-metrics" ? null : 720,
            DailyEstimatedCostUsd: 12.40m, CriticalRisk: caseId == "policy-block");
        string evidenceHash = Convert.ToHexString(SHA256.HashData(
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evidence)))).ToLowerInvariant();
        var v1 = Execute("v1", evidence);
        var v2 = Execute("v2", evidence);
        var events = new List<ErikaEvent>();
        void Emit(string kind, string message, string gate = "NONE") =>
            events.Add(new(events.Count + 1, kind, message, gate));

        Emit("mission.started", "SYNTHETIC Container Apps evidence; no Azure requests or cloud writes.");
        Emit("workflow.v1.completed", $"v1 executed {v1.ToolCalls} local tool calls.");
        Emit("workflow.v2.completed", $"v2 executed {v2.ToolCalls} local tool calls.");
        var quality = Math.Min(v1.QualityScore, v2.QualityScore);
        var callsSavedPercent = 100.0 * (v1.ToolCalls - v2.ToolCalls) / v1.ToolCalls;
        var durationSavedPercent = 100.0 * (v1.SyntheticDurationMs - v2.SyntheticDurationMs) / v1.SyntheticDurationMs;
        string proposal = caseId == "policy-block"
            ? "Proposed minReplicas=0 would violate the critical safety floor. No change allowed."
            : "Propose minReplicas 3 → 1 under load-based scaling, retain maxReplicas=12 and HTTP concurrency=50; preserve p95 SLO.";
        string decision;
        string reason;
        string outcome;
        LocalRollbackProof? proof = null;

        // Backend deterministic gates; missing data is DENY, never defaulted to zero.
        if (evidence.P95LatencyMs is null)
        {
            decision = "DENY";
            reason = "MISSING_REQUIRED_LATENCY_METRIC";
            outcome = "BLOCKED";
        }
        else if (quality < 0.90)
        {
            decision = "DENY";
            reason = "QUALITY_BELOW_90_PERCENT";
            outcome = "BLOCKED";
        }
        else if (callsSavedPercent < 20 || durationSavedPercent < 15)
        {
            decision = "DENY";
            reason = "OPTIMIZATION_TARGET_NOT_MET";
            outcome = "BLOCKED";
        }
        else
        {
            var result = policy.Evaluate(estimatedUsd: 0.42, quality: quality,
                latencyMs: evidence.P95LatencyMs.Value, criticalRisk: evidence.CriticalRisk);
            decision = result.Allowed ? "PASS" : "DENY";
            reason = result.Reason;
            outcome = result.Allowed ? "AWAITING_HUMAN_REVIEW" : "BLOCKED";
        }
        Emit("quality.evaluated", $"Rubric: {quality:P0}; logical calls saved: {callsSavedPercent:F1}%; synthetic modeled duration saved: {durationSavedPercent:F1}%.", decision);
        Emit("policy.evaluated", reason, decision);

        if (decision == "DENY")
            Emit("mission.blocked", "No approval, no publication and no deployment.", "DENY");
        else if (caseId == "quality-rollback")
        {
            // Local sandbox: actually switch an on-disk workflow manifest to v2,
            // simulate a late quality regression, restore v1 and verify SHA-256.
            // This is explicitly NOT an approved production deployment.
            Emit("quality.regression", "SYNTHETIC late quality regression: score 0.73.", "DENY");
            proof = LocalWorkflowRollback.RestoreV1(
                rollbackDirectory ?? Path.Combine(Path.GetTempPath(), "merlinops-erika-" + Guid.NewGuid().ToString("N")));
            outcome = proof.Verified ? "ROLLED_BACK_LOCAL" : "RECOVERY_FAILED";
            Emit(proof.Verified ? "rollback.verified" : "rollback.failed",
                proof.Verified ? "Actual local workflow manifest restored to v1 and SHA-256 verified." : "Local workflow restoration could not be verified.",
                proof.Verified ? "PASS" : "DENY");
        }
        else
            Emit("approval.required", "Technical gates PASS. No authorized human approval exists; no publication or Azure write.", "PENDING");

        return new ErikaComparison(
            Id: Guid.NewGuid().ToString("N"), CaseId: caseId, Provenance: "SIMULATED",
            EvidenceSource: "SYNTHETIC_FIXTURE", TargetResource: evidence.Resource,
            UserRequest: "Diagnose high Azure Container Apps cost and irregular latency; propose safe autoscaling changes without applying them.",
            ExpectedResult: "Evidence-backed recommendation, reproducible v1/v2 benchmark and mandatory policy/human gates.",
            EvidenceSha256: evidenceHash, V1: v1, V2: v2,
            CallReductionPercent: Math.Round(callsSavedPercent, 1),
            SyntheticDurationReductionPercent: Math.Round(durationSavedPercent, 1),
            ProposedChange: proposal, TechnicalDecision: decision, DecisionReason: reason,
            Outcome: outcome, HumanApproval: "NOT_REQUESTED", CloudWritesEnabled: false,
            Rollback: proof, Events: events);
    }

    private static ErikaVersionResult Execute(string version, AppEvidence source)
    {
        var tools = new LocalEvidenceTools();
        if (version == "v1")
        {
            // Deliberately redundant checks in baseline agent-driven flow.
            tools.Configuration();
            tools.Traffic();
            tools.Latency();
            tools.Configuration();
            tools.Scaling();
            tools.Traffic();
            tools.Cost();
            tools.Policy();
            tools.Latency();
        }
        else
        {
            // Explicit workflow: one structured operation per evidence group.
            tools.ConfigurationAndScaling();
            tools.Metrics();
            tools.Cost();
            tools.Policy();
        }
        double score = source.P95LatencyMs.HasValue ? 1.00 : 0.75;
        int duration = tools.Calls.Count * 150 + (version == "v1" ? 60 : 90);
        return new ErikaVersionResult(version, tools.Calls.Count, duration, "SYNTHETIC_MODEL",
            score, tools.Calls.ToArray());
    }

    private sealed record AppEvidence(string Resource, int MinReplicas, int MaxReplicas,
        int HttpConcurrency, int Requests24h, int? P95LatencyMs, decimal DailyEstimatedCostUsd, bool CriticalRisk);

    private sealed class LocalEvidenceTools
    {
        public List<string> Calls { get; } = [];
        public void Configuration() => Calls.Add("GET configuration");
        public void Scaling() => Calls.Add("GET scaling");
        public void Traffic() => Calls.Add("GET request-volume");
        public void Latency() => Calls.Add("GET latency-p95");
        public void Cost() => Calls.Add("GET estimated-cost");
        public void Policy() => Calls.Add("GET policy");
        public void ConfigurationAndScaling() => Calls.Add("GET configuration+scaling");
        public void Metrics() => Calls.Add("GET traffic+latency");
    }
}

public sealed record ErikaVersionResult(string Version, int ToolCalls, int SyntheticDurationMs,
    string DurationProvenance, double QualityScore, IReadOnlyList<string> ToolTrace);
public sealed record ErikaEvent(int Sequence, string Kind, string Message, string Gate);
public sealed record LocalRollbackProof(string RestoredVersion, string ExpectedSha256,
    string ActualSha256, bool Verified);
public sealed record ErikaComparison(string Id, string CaseId, string Provenance, string EvidenceSource,
    string TargetResource, string UserRequest, string ExpectedResult, string EvidenceSha256,
    ErikaVersionResult V1, ErikaVersionResult V2, double CallReductionPercent,
    double SyntheticDurationReductionPercent, string ProposedChange, string TechnicalDecision,
    string DecisionReason, string Outcome, string HumanApproval, bool CloudWritesEnabled,
    LocalRollbackProof? Rollback, IReadOnlyList<ErikaEvent> Events);

/// <summary>Real on-disk *local* version restoration. No Azure or GitHub modifications.</summary>
public static class LocalWorkflowRollback
{
    public static LocalRollbackProof RestoreV1(string directory)
    {
        Directory.CreateDirectory(directory);
        var v1 = Path.Combine(directory, "workflow-v1.json");
        var v2 = Path.Combine(directory, "workflow-v2.json");
        var active = Path.Combine(directory, "workflow-active.json");
        File.WriteAllText(v1, "{\"version\":\"v1\",\"mode\":\"local\",\"minReplicas\":3}");
        File.WriteAllText(v2, "{\"version\":\"v2\",\"mode\":\"local\",\"minReplicas\":1}");
        File.Copy(v2, active, true);
        if (!File.ReadAllText(active).Contains("\"v2\"", StringComparison.Ordinal))
            throw new InvalidOperationException("Local v2 activation not verified");
        File.Copy(v1, active, true);
        string Hash(string path) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();
        var expected = Hash(v1);
        var actual = Hash(active);
        var verified = expected == actual && File.ReadAllText(active).Contains("\"v1\"", StringComparison.Ordinal);
        return new("v1", expected, actual, verified);
    }
}

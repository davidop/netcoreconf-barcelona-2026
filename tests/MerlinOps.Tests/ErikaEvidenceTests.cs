using MerlinOps.Api;
using Xunit;

namespace MerlinOps.Tests;

/// <summary>Erika's acceptance criteria: actual local calls, deterministic gates, rollback proof.</summary>
public sealed class ErikaEvidenceTests
{
    private static ErikaOptimizationEngine Engine() =>
        new(new PolicyEngine(Path.Combine(AppContext.BaseDirectory, "guardrails.yaml")));

    [Fact]
    public void Optimization_uses_identical_evidence_and_reduces_actual_local_tool_calls()
    {
        var result = Engine().Compare("finops-success");
        Assert.Equal("SIMULATED", result.Provenance);
        Assert.Equal("SYNTHETIC_FIXTURE", result.EvidenceSource);
        Assert.Equal(9, result.V1.ToolCalls);
        Assert.Equal(4, result.V2.ToolCalls);
        Assert.True(result.CallReductionPercent >= 20);
        Assert.True(result.SyntheticDurationReductionPercent >= 15);
        Assert.Equal("SYNTHETIC_MODEL", result.V1.DurationProvenance);
        Assert.Equal("SYNTHETIC_MODEL", result.V2.DurationProvenance);
        Assert.True(result.V2.QualityScore >= 0.90);
        Assert.Equal("PASS", result.TechnicalDecision);
        Assert.Equal("AWAITING_HUMAN_REVIEW", result.Outcome);
        Assert.Equal("NOT_REQUESTED", result.HumanApproval);
        Assert.False(result.CloudWritesEnabled);
    }

    [Theory]
    [InlineData("policy-block", "CRITICAL_RISK")]
    [InlineData("missing-metrics", "MISSING_REQUIRED_LATENCY_METRIC")]
    public void Policy_and_missing_metrics_fail_closed_without_approval(string scenario, string reason)
    {
        var result = Engine().Compare(scenario);
        Assert.Equal("DENY", result.TechnicalDecision);
        Assert.Equal(reason, result.DecisionReason);
        Assert.Equal("BLOCKED", result.Outcome);
        Assert.DoesNotContain(result.Events, e => e.Kind == "approval.required");
        Assert.DoesNotContain(result.Events, e => e.Kind.StartsWith("deployment.", StringComparison.Ordinal));
        Assert.Null(result.Rollback);
    }

    [Fact]
    public void Rollback_actually_restores_the_local_active_manifest_and_verifies_integrity()
    {
        var directory = Path.Combine(Path.GetTempPath(), "merlinops-erika-test-" + Guid.NewGuid().ToString("N"));
        try
        {
            var result = Engine().Compare("quality-rollback", directory);
            Assert.Equal("ROLLED_BACK_LOCAL", result.Outcome);
            Assert.NotNull(result.Rollback);
            Assert.True(result.Rollback.Verified);
            Assert.Equal(result.Rollback.ExpectedSha256, result.Rollback.ActualSha256);
            Assert.Equal("v1", result.Rollback.RestoredVersion);
            Assert.Equal(File.ReadAllText(Path.Combine(directory, "workflow-v1.json")),
                         File.ReadAllText(Path.Combine(directory, "workflow-active.json")));
            Assert.Contains(result.Events, e => e.Kind == "quality.regression");
            Assert.Contains(result.Events, e => e.Kind == "rollback.verified");
        }
        finally { if (Directory.Exists(directory)) Directory.Delete(directory, true); }
    }

    [Fact]
    public void Minimum_replica_policy_is_enforced_by_the_backend_not_the_model()
    {
        var policy = new PolicyEngine(Path.Combine(AppContext.BaseDirectory, "guardrails.yaml"));
        Assert.False(policy.EvaluateContainerAppScaling(0.42, 1.0, 720, 0, 12).Allowed);
        Assert.False(policy.EvaluateContainerAppScaling(0.42, 1.0, 720, 3, 1).Allowed);
        Assert.True(policy.EvaluateContainerAppScaling(0.42, 1.0, 720, 1, 12).Allowed);
    }

    [Fact]
    public void All_comparisons_provide_ordered_auditable_events()
    {
        foreach (var scenario in ErikaOptimizationEngine.Cases)
        {
            var directory = Path.Combine(Path.GetTempPath(), "merlinops-erika-events-" + Guid.NewGuid().ToString("N"));
            try
            {
                var result = Engine().Compare(scenario, directory);
                Assert.Equal(Enumerable.Range(1, result.Events.Count), result.Events.Select(e => e.Sequence));
                Assert.Equal(64, result.EvidenceSha256.Length);
                Assert.All(result.Events, e => Assert.NotEqual("", e.Message));
            }
            finally { if (Directory.Exists(directory)) Directory.Delete(directory, true); }
        }
    }
}

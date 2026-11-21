using MerlinOps.Api;
using Xunit;

namespace MerlinOps.Tests;

public sealed class IncidentImprovementTests
{
    private static IncidentImprovementEngine Create() =>
        new(new PolicyEngine(Path.Combine(AppContext.BaseDirectory, "guardrails.yaml")));

    [Fact]
    public async Task Both_versions_execute_real_maf_guardians_and_count_local_tools()
    {
        var report = await Create().CompareAsync("incident-improvement");
        Assert.Equal("SIMULATED", report.Mode);
        Assert.Equal("SYNTHETIC_FIXTURE", report.EvidenceSource);
        Assert.Equal("INC-2042", report.IncidentId);
        Assert.Equal(9, report.V1.ToolCalls);
        Assert.Equal(4, report.V2.ToolCalls);
        Assert.Equal(4, report.V1.MafGuardiansCompleted);
        Assert.Equal(4, report.V2.MafGuardiansCompleted);
        Assert.Equal("LOCAL_MEASURED_SYNTHETIC_TOOLS", report.V1.DurationProvenance);
        Assert.True(report.V1.ObservedLocalDurationMs > 0);
        Assert.True(report.V2.ObservedLocalDurationMs > 0);
        Assert.Equal(55.6, report.ToolCallReductionPercent);
        Assert.Equal("PASS", report.TechnicalDecision);
        Assert.Equal("AWAITING_HUMAN_REVIEW", report.Outcome);
        Assert.Equal("NOT_REQUESTED", report.HumanApproval);
        Assert.False(report.CloudWritesEnabled);
        Assert.Equal("NOT_CONNECTED", report.Foundry.Status);
        Assert.Null(report.Foundry.InputTokens);
        Assert.Null(report.Foundry.CostUsd);
        Assert.Contains(report.Audit, e => e.Kind == "approval.required");
        Assert.Equal(64, report.EvidenceSha256.Length);
    }

    [Theory]
    [InlineData("incident-policy-block", "INCIDENT_RETRY_LIMIT")]
    [InlineData("incident-missing-evidence", "MISSING_REQUIRED_TRACE")]
    public async Task Negative_cases_deny_before_any_approval(string caseId, string reason)
    {
        var report = await Create().CompareAsync(caseId);
        Assert.Equal("BLOCKED", report.Outcome);
        Assert.Equal("DENY", report.TechnicalDecision);
        Assert.Equal(reason, report.DecisionReason);
        Assert.DoesNotContain(report.Audit, e => e.Kind.StartsWith("approval.", StringComparison.Ordinal));
        Assert.DoesNotContain(report.Audit, e => e.Kind.StartsWith("deployment.", StringComparison.Ordinal));
        Assert.Null(report.Rollback);
    }

    [Fact]
    public async Task Local_incident_workflow_recovers_from_regression_and_checks_sha256()
    {
        var dir = Path.Combine(Path.GetTempPath(), "merlinops-incident-test-" + Guid.NewGuid().ToString("N"));
        try
        {
            var report = await Create().CompareAsync("incident-regression", dir);
            Assert.Equal("ROLLED_BACK_LOCAL", report.Outcome);
            Assert.Equal("PASS", report.TechnicalDecision);
            Assert.NotNull(report.Rollback);
            Assert.True(report.Rollback.Verified);
            Assert.Equal("v1", report.Rollback.RestoredVersion);
            Assert.Equal(report.Rollback.ExpectedSha256, report.Rollback.ActualSha256);
            Assert.Equal(File.ReadAllText(Path.Combine(dir, "incident-v1.json")),
                         File.ReadAllText(Path.Combine(dir, "incident-active.json")));
            Assert.Contains(report.Audit, e => e.Kind == "quality.regression");
            Assert.Contains(report.Audit, e => e.Kind == "rollback.verified");
            Assert.DoesNotContain(report.Audit, e => e.Kind.StartsWith("deployment.", StringComparison.Ordinal));
        }
        finally { if (Directory.Exists(dir)) Directory.Delete(dir, recursive: true); }
    }

    [Fact]
    public async Task All_cases_emit_monotonic_audit_events_with_truthful_source()
    {
        foreach (var testCase in IncidentImprovementEngine.Cases)
        {
            var dir = Path.Combine(Path.GetTempPath(), "merlinops-incident-case-" + Guid.NewGuid().ToString("N"));
            try
            {
                var report = await Create().CompareAsync(testCase, dir);
                Assert.Equal(Enumerable.Range(1, report.Audit.Count), report.Audit.Select(x => x.Sequence));
                Assert.All(report.Audit, e => Assert.Equal("SIMULATED", e.Provenance));
                Assert.All(report.Audit, e => Assert.Equal(report.Id, e.TraceId));
                Assert.All(report.Audit, e => Assert.False(string.IsNullOrWhiteSpace(e.Message)));
                Assert.Equal(report.V1.QualityScore, report.V2.QualityScore);
            }
            finally { if (Directory.Exists(dir)) Directory.Delete(dir, recursive: true); }
        }
    }

    [Fact]
    public void Retry_budget_is_independent_of_model_text_and_fails_closed()
    {
        var policy = new PolicyEngine(Path.Combine(AppContext.BaseDirectory, "guardrails.yaml"));
        Assert.False(policy.EvaluateIncidentWorkflow(1.0, 8).Allowed);
        Assert.False(policy.EvaluateIncidentWorkflow(1.0, -1).Allowed);
        Assert.False(policy.EvaluateIncidentWorkflow(double.NaN, 2).Allowed);
        Assert.False(policy.EvaluateIncidentWorkflow(0.89, 2).Allowed);
        Assert.True(policy.EvaluateIncidentWorkflow(1.0, 2).Allowed);
    }
}

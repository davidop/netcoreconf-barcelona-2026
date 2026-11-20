using MerlinOps.Api;

namespace MerlinOps.Tests;

public sealed class SimulationTests
{
    private static PolicyEngine Policy() =>
        new(Path.Combine(AppContext.BaseDirectory, "guardrails.yaml"));

    [Theory]
    [InlineData("finops-success", "COMPLETED")]
    [InlineData("policy-block", "BLOCKED")]
    [InlineData("quality-rollback", "ROLLED_BACK")]
    public void Three_scenarios_have_truthful_outcomes(string scenario, string expected)
    {
        var run = new SimulationEngine(Policy()).Execute(scenario);
        Assert.Equal(expected, run.Status);
        Assert.Equal("SIMULATED", run.Mode);
        Assert.Equal(1, run.Events[0].Sequence);
        Assert.Equal(Enumerable.Range(1, run.Events.Count), run.Events.Select(x => x.Sequence));
        Assert.All(run.Events, evt => Assert.Equal(run.Events[0].TraceId, evt.TraceId));
    }

    [Fact]
    public void Critical_policy_blocks_without_approval_or_deployment()
    {
        var run = new SimulationEngine(Policy()).Execute("policy-block");
        Assert.Contains(run.Events, e => e.Kind == "gate.evaluated" && e.Gate == "DENY");
        Assert.DoesNotContain(run.Events, e => e.Kind.StartsWith("deployment."));
        Assert.DoesNotContain(run.Events, e => e.Kind.StartsWith("approval."));
    }

    [Fact]
    public void Auto_rollback_only_after_synthetic_quality_regression()
    {
        var run = new SimulationEngine(Policy()).Execute("quality-rollback");
        var regression = run.Events.Single(e => e.Kind == "telemetry.regression");
        var rollback = run.Events.Single(e => e.Kind == "rollback.simulated");
        Assert.True(regression.Sequence < rollback.Sequence);
        Assert.Equal(0.73, regression.Metric);
    }

    [Fact]
    public void Cost_gate_fails_closed_above_limit()
    {
        var policy = Policy();
        Assert.False(policy.Evaluate(1.01, 0.95, 1500, false).Allowed);
        Assert.False(policy.Evaluate(0.1, 0.5, 1500, false).Allowed);
        Assert.False(policy.Evaluate(0.1, 0.95, 1500, true).Allowed);
        Assert.True(policy.Evaluate(0.1, 0.95, 1500, false).Allowed);
    }

    [Fact]
    public async Task SQLite_persists_and_restores_the_run()
    {
        var path = Path.Combine(Path.GetTempPath(), $"merlinops-tests-{Guid.NewGuid():N}.db");
        try
        {
            var run = new SimulationEngine(Policy()).Execute("finops-success");
            await new SqliteRunStore(path).SaveAsync(run);
            var restored = await new SqliteRunStore(path).GetAsync(run.Id);
            Assert.NotNull(restored);
            Assert.Equal(run.Events.Count, restored.Events.Count);
            Assert.Equal(run.Status, restored.Status);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }
}

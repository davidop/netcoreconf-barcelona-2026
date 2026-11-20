using Microsoft.Agents.AI.Workflows;

namespace MerlinOps.Api;

/// <summary>
/// REAL Microsoft Agent Framework fan-out/fan-in graph execution, with purely
/// deterministic local Guardian executors. No model or cloud integrations.
/// Every event is marked SIMULATED; authorization is still enforced outside MAF.
/// </summary>
public sealed class MafSimulationEngine(PolicyEngine policy)
{
    public async Task<RunSnapshot> ExecuteAsync(string scenarioId)
    {
        if (!SimulationEngine.Scenarios.Any(s => s.Id == scenarioId))
            throw new ArgumentException("Unknown scenario", nameof(scenarioId));

        Func<string, string> plan = input => "Synthetic planning for " + input;
        Func<string, string> telemetry = _ => "p95 latency (synthetic): 1850ms";
        Func<string, string> finops = _ => "Estimated cost (synthetic): USD 0.42";
        Func<string, string> quality = _ => "Quality score (synthetic): 0.94";
        Func<string, string> governance = _ => "Mandatory approval and deny-on-critical-risk policies";

        var planner = plan.BindAsExecutor("MerlinOpsPlanner");
        var guardians = new[]
        {
            telemetry.BindAsExecutor("TelemetryGuardian"),
            finops.BindAsExecutor("FinOpsGuardian"),
            quality.BindAsExecutor("QualityGuardian"),
            governance.BindAsExecutor("GovernanceGuardian"),
        };

        var builder = new WorkflowBuilder(planner);
        builder.AddFanOutEdge(planner, guardians).WithOutputFrom(guardians);
        var workflow = builder.Build();

        var events = new List<MissionEvent>();
        var trace = Guid.NewGuid().ToString("N");
        void Emit(string kind, string agent, string text, string gate = "NONE", double? metric = null) =>
            events.Add(new(events.Count + 1, kind, agent, text, gate, metric, trace));

        Emit("mission.started", "MerlinOps", "SIMULATED MAF workflow started. Cloud writes disabled.");
        Emit("delegation.parallel", "MerlinOps", "Microsoft Agent Framework fan-out to 4 deterministic Guardians.");

        await using Run run = await InProcessExecution.RunAsync(workflow, scenarioId);

        HashSet<string> seenGuardians = new(StringComparer.Ordinal);
        foreach (WorkflowEvent evt in run.NewEvents)
        {
            if (evt is ExecutorCompletedEvent complete)
            {
                var actor = complete.ExecutorId;
                if (actor == "MerlinOpsPlanner")
                {
                    Emit("plan.created", "MerlinOps", "MAF planner completed a deterministic plan.");
                }
                else if (actor.EndsWith("Guardian", StringComparison.Ordinal))
                {
                    if (!seenGuardians.Add(actor))
                        throw new InvalidOperationException("Duplicate Guardian completion; fail closed.");
                    Emit("agent.completed", actor, Convert.ToString(complete.Data) ?? "");
                }
            }
        }
        if (seenGuardians.Count != 4)
            throw new InvalidOperationException("Missing Guardian completion; fail closed before approval.");

        // The policy engine is deliberately outside the agent framework graph;
        // a model or agent can never override these deterministic conditions.
        GuardDecision decision = policy.Evaluate(
            estimatedUsd: 0.42, quality: 0.94, latencyMs: 1850, criticalRisk: scenarioId == "policy-block");
        Emit("gate.evaluated", "GovernanceGuardian",
            decision.Allowed ? "Deterministic policy passed; verified human approval remains required."
                             : "Critical policy denied proposed change.",
            decision.Allowed ? "PASS" : "DENY");

        if (!decision.Allowed)
        {
            Emit("mission.blocked", "MerlinOps", "Blocked. No approval and no deployment.", "DENY");
            return new(Guid.NewGuid().ToString("N"), scenarioId, "SIMULATED",
                "BLOCKED", DateTimeOffset.UtcNow, events);
        }

        Emit("approval.requested", "GovernanceGuardian",
            "Synthetic approval requirement; no actual Teams message sent.", "PENDING");
        Emit("approval.simulated", "HumanReviewer",
            "Fictional approval for storytelling; NOT an authorization.", "SIMULATED");
        Emit("deployment.simulated", "DeliveryGuardian",
            "Fictional progressive deployment 10% → 50% → 100%. No Azure writes.", "SIMULATED");

        if (scenarioId == "quality-rollback")
        {
            Emit("telemetry.regression", "QualityGuardian", "Synthetic quality regression to 0.73.", "DENY", 0.73);
            Emit("rollback.simulated", "DeliveryGuardian",
                "Fictional preauthorized rollback. No Azure writes.", "SIMULATED");
            Emit("mission.recovered", "MerlinOps", "Synthetic quality restored.", "PASS");
            return new(Guid.NewGuid().ToString("N"), scenarioId, "SIMULATED",
                "ROLLED_BACK", DateTimeOffset.UtcNow, events);
        }

        Emit("finops.projected", "FinOpsGuardian",
            "Synthetic projected cost reduction of 24%, not measured savings.", "PASS", 24);
        Emit("mission.completed", "MerlinOps", "MAF simulated mission completed.", "PASS");
        return new(Guid.NewGuid().ToString("N"), scenarioId, "SIMULATED",
            "COMPLETED", DateTimeOffset.UtcNow, events);
    }
}

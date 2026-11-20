namespace MerlinOps.Api;

/// <summary>
/// A reproducible simulated mission. No real Azure, model, Teams, or GitHub mutation occurs.
/// The three scenarios are correlated event sequences rather than a live MAF execution.
/// </summary>
public sealed class SimulationEngine(PolicyEngine policy)
{
    public static readonly IReadOnlyList<ScenarioInfo> Scenarios =
    [
        new("finops-success", "El hechizo del ahorro", "Model routing, cost/latency/quality gates and human approval", "LIVE_OR_REPLAY"),
        new("policy-block", "El escudo de gobernanza", "Fail-closed critical policy blocks an unsafe recommendation", "LIVE_OR_REPLAY"),
        new("quality-rollback", "El hechizo que salió mal", "Post-change quality regression triggers pre-authorized auto-rollback", "REPLAY")
    ];

    public RunSnapshot Execute(string scenarioId)
    {
        if (!Scenarios.Any(s => s.Id == scenarioId))
            throw new ArgumentException("Unknown scenario", nameof(scenarioId));

        var events = new List<MissionEvent>();
        string trace = Guid.NewGuid().ToString("N");
        void Emit(string kind, string agent, string message, string gate = "NONE", double? metric = null)
            => events.Add(new(events.Count + 1, kind, agent, message, gate, metric, trace));

        Emit("mission.started", "MerlinOps", "SIMULATED mission started. No cloud resources are accessed.");
        Emit("plan.created", "MerlinOps", "Optimize Foundry model routing under quality, latency and cost constraints.");
        Emit("delegation.parallel", "MerlinOps", "Dispatching Telemetry, FinOps, Quality and Governance guardians.");
        Emit("agent.observed", "TelemetryGuardian", "Synthetic p95 latency: 1850 ms.", metric: 1850);
        Emit("agent.estimated", "FinOpsGuardian", "Synthetic cost estimate: USD 0.42 per workflow.", metric: 0.42);
        Emit("agent.evaluated", "QualityGuardian", "Synthetic quality score: 0.94.", metric: 0.94);

        bool critical = scenarioId == "policy-block";
        var gate = policy.Evaluate(0.42, 0.94, 1850, critical);
        Emit("gate.evaluated", "GovernanceGuardian",
            gate.Allowed ? "Policy gate passed; explicit human approval required." : "Critical risk detected. Blocking the proposed change.",
            gate.Allowed ? "PASS" : "DENY");

        if (!gate.Allowed)
        {
            Emit("mission.blocked", "MerlinOps", "Rejected by fail-closed policy. No deployment was performed.", "DENY");
            return new(Guid.NewGuid().ToString("N"), scenarioId, "SIMULATED", "BLOCKED", DateTimeOffset.UtcNow, events);
        }

        Emit("approval.requested", "GovernanceGuardian", "Synthetic approval request (Teams is not contacted).", "PENDING");
        Emit("approval.simulated", "HumanReviewer", "Synthetic pre-recorded APPROVED decision for demonstration only.", "SIMULATED");
        Emit("deployment.simulated", "DeliveryGuardian", "Synthetic progressive deployment: 10% -> 50% -> 100%.", "SIMULATED");

        if (scenarioId == "quality-rollback")
        {
            Emit("telemetry.regression", "QualityGuardian", "Synthetic post-deployment quality drops to 0.73.", "DENY", 0.73);
            Emit("rollback.simulated", "DeliveryGuardian", "Synthetic pre-authorized rollback to prior model configuration.", "SIMULATED");
            Emit("mission.recovered", "MerlinOps", "Synthetic quality restored. No cloud changes occurred.", "PASS");
            return new(Guid.NewGuid().ToString("N"), scenarioId, "SIMULATED", "ROLLED_BACK", DateTimeOffset.UtcNow, events);
        }

        Emit("telemetry.verified", "QualityGuardian", "Synthetic post-deployment quality: 0.95.", "PASS", 0.95);
        Emit("finops.projected", "FinOpsGuardian", "Synthetic projected relative cost reduction: 24%. Not realized savings.", "PASS", 24);
        Emit("mission.completed", "MerlinOps", "Scenario completed in SIMULATED mode.", "PASS");
        return new(Guid.NewGuid().ToString("N"), scenarioId, "SIMULATED", "COMPLETED", DateTimeOffset.UtcNow, events);
    }
}

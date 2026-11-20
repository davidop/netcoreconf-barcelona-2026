using Microsoft.Agents.AI.Workflows;

// Real Microsoft Agent Framework fan-out execution with deterministic local specialists.
// No model invocation, Azure access, Teams messages or infrastructure writes occur here.
// Stage 1: MerlinOps plans. Stage 2: four specialist executors receive the plan in parallel.
// Actual AI planning, model routing and back-end policy enforcement remain separate future tasks.
Func<string, string> plan = request => "PLAN: Optimize Foundry model routing for " + request;
Func<string, string> telemetry = input => "TelemetryGuardian synthetic observation: p95=1850ms; " + input;
Func<string, string> finops = input => "FinOpsGuardian synthetic estimate: USD 0.42; " + input;
Func<string, string> quality = input => "QualityGuardian synthetic score: 0.94; " + input;
Func<string, string> governance = input => "GovernanceGuardian: server-side gates and verified human approval required; " + input;

var planner = plan.BindAsExecutor("MerlinOpsPlanner");
var guardians = new[]
{
    telemetry.BindAsExecutor("TelemetryGuardian"),
    finops.BindAsExecutor("FinOpsGuardian"),
    quality.BindAsExecutor("QualityGuardian"),
    governance.BindAsExecutor("GovernanceGuardian"),
};

var builder = new WorkflowBuilder(planner);
builder.AddFanOutEdge(planner, guardians)
       .WithOutputFrom(guardians);
var workflow = builder.Build();

await using Run run = await InProcessExecution.RunAsync(workflow, "SYNTHETIC_ONLY");

int guardianCompletions = 0;
foreach (WorkflowEvent evt in run.NewEvents)
{
    if (evt is ExecutorCompletedEvent completed)
    {
        Console.WriteLine($"{completed.ExecutorId}: {completed.Data}");
        if (completed.ExecutorId.EndsWith("Guardian", StringComparison.Ordinal)) guardianCompletions++;
    }
}

if (guardianCompletions != 4)
    throw new InvalidOperationException($"Expected four Guardian results, got {guardianCompletions}.");

Console.WriteLine("MAF_FANOUT_SMOKE_COMPLETE: 4 deterministic Guardian branches, no cloud operations.");

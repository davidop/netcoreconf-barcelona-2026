using Microsoft.Agents.AI.Workflows;

// Isolated SDK Preview smoke test. Demonstrates the workflow graph API.
// Does NOT call Foundry, Copilot Studio, Teams or Azure.
Func<string, string> plan = input => "PLAN:" + input;
Func<string, string> govern = input => input + ":APPROVAL_REQUIRED";

var planner = plan.BindAsExecutor("MerlinOpsPlanner");
var gate = govern.BindAsExecutor("GovernanceGate");
WorkflowBuilder builder = new(planner);
builder.AddEdge(planner, gate).WithOutputFrom(gate);
var workflow = builder.Build();

await using Run run = await InProcessExecution.RunAsync(workflow, "SYNTHETIC_ONLY");
foreach (WorkflowEvent evt in run.NewEvents)
{
    if (evt is ExecutorCompletedEvent completed)
    {
        Console.WriteLine($"{completed.ExecutorId}: {completed.Data}");
    }
}
Console.WriteLine("MAF_PREVIEW_SMOKE_COMPLETE (no cloud calls)");

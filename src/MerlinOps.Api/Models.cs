namespace MerlinOps.Api;

/// <summary>
/// Event schema shared by the simulated runtime, persisted execution and Slidev Replay.
/// All event payloads here are synthetic and explicitly labelled SIMULATED.
/// </summary>
public sealed record MissionEvent(
    int Sequence,
    string Kind,
    string Agent,
    string Message,
    string Gate = "NONE",
    double? Metric = null,
    string? TraceId = null);

public sealed record RunSnapshot(
    string Id,
    string Scenario,
    string Mode,
    string Status,
    DateTimeOffset CreatedAt,
    List<MissionEvent> Events);

public sealed record ScenarioInfo(string Id, string Name, string Objective, string IntendedStageMode);

public sealed record GuardDecision(bool Allowed, string Reason);

public sealed class Guardrails
{
    public double MaxEstimatedUsdPerRun { get; set; } = 1.0;
    public double MinQualityScore { get; set; } = 0.85;
    public int MaxLatencyMs { get; set; } = 3000;
    public bool HumanApprovalRequired { get; set; } = true;
    public bool AllowCloudWrites { get; set; } = false;
}

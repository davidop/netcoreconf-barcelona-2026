using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MerlinOps.Api;

/// <summary>
/// Local Policy-as-Code executor. Fail closed on malformed or missing policy.
/// Not an OPA, Azure Policy or Entra implementation.
/// </summary>
public sealed class PolicyEngine
{
    public Guardrails Rules { get; }

    public PolicyEngine(string policyPath)
    {
        if (!File.Exists(policyPath))
        {
            throw new InvalidOperationException("Guardrails file missing; refusing to start.");
        }

        var parser = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();
        Rules = parser.Deserialize<Guardrails>(File.ReadAllText(policyPath))
            ?? throw new InvalidOperationException("Guardrails policy empty.");
        if (Rules.MaxEstimatedUsdPerRun <= 0 || Rules.MinQualityScore is < 0 or > 1
            || Rules.MaxLatencyMs <= 0 || Rules.MinAllowedReplicas < 1
            || Rules.MaxIncidentRetries is < 1 or > 10
            || !Rules.HumanApprovalRequired || Rules.AllowCloudWrites)
        {
            throw new InvalidOperationException("Guardrails weaken mandatory demo safety controls.");
        }
    }

    /// <summary>Never let a model, agent or presentation bypass the incident safety gate.</summary>
    public GuardDecision EvaluateIncidentWorkflow(double quality, int proposedRetries)
    {
        if (proposedRetries < 0 || proposedRetries > Rules.MaxIncidentRetries)
            return new(false, "INCIDENT_RETRY_LIMIT");
        if (double.IsNaN(quality) || double.IsInfinity(quality) || quality is < 0 or > 1)
            return new(false, "INVALID_QUALITY_SCORE");
        if (quality < 0.90) return new(false, "QUALITY_BELOW_90_PERCENT");
        return new(true, "POLICY_PASSED_APPROVAL_REQUIRED");
    }

    /// <summary>Enforce autoscaling safety in the backend, not in the model response.</summary>
    public GuardDecision EvaluateContainerAppScaling(double estimatedUsd, double quality,
        int latencyMs, int proposedMinReplicas, int proposedMaxReplicas)
    {
        if (proposedMinReplicas < Rules.MinAllowedReplicas || proposedMaxReplicas < proposedMinReplicas)
            return new(false, "CRITICAL_RISK");
        return Evaluate(estimatedUsd, quality, latencyMs, criticalRisk: false);
    }

    public GuardDecision Evaluate(double estimatedUsd, double quality, int latencyMs, bool criticalRisk)
    {
        if (criticalRisk) return new(false, "CRITICAL_RISK");
        if (estimatedUsd > Rules.MaxEstimatedUsdPerRun) return new(false, "COST_LIMIT");
        if (quality < Rules.MinQualityScore) return new(false, "QUALITY_LIMIT");
        if (latencyMs > Rules.MaxLatencyMs) return new(false, "LATENCY_LIMIT");
        return new(true, "POLICY_PASSED_APPROVAL_REQUIRED");
    }
}

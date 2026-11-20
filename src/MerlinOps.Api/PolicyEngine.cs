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
            || Rules.MaxLatencyMs <= 0 || !Rules.HumanApprovalRequired || Rules.AllowCloudWrites)
        {
            throw new InvalidOperationException("Guardrails weaken mandatory demo safety controls.");
        }
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

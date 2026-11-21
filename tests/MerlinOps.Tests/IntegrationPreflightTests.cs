using MerlinOps.Api;
using Xunit;

namespace MerlinOps.Tests;

public sealed class IntegrationPreflightTests
{
    [Fact]
    public void Empty_configuration_never_claims_connected_cloud_products()
    {
        var status = MerlinOpsIntegrationPreflight.Inspect(null, null, null);
        Assert.Equal("NOT_CONNECTED", status.CopilotStudio);
        Assert.Equal("NOT_CONNECTED", status.Foundry);
        Assert.Equal("A2A_PENDING_VALIDATION", status.PreferredProtocol);
        Assert.False(status.FoundryEndpointShapeValidated);
        Assert.False(status.ModelPairSpecified);
        Assert.False(status.CloudWritesEnabled);
        Assert.Equal("UNAVAILABLE_UNTIL_VERIFIED_INFERENCE", status.ModelUsageProvenance);
    }

    [Theory]
    [InlineData("http://test.services.ai.azure.com/api/projects/p", false)]
    [InlineData("https://evil.example.org/api/projects/p", false)]
    [InlineData("https://user:password@test.services.ai.azure.com/api/projects/p", false)]
    [InlineData("https://test.services.ai.azure.com/api/projects/p?api_key=not-a-secret", false)]
    [InlineData("https://test.services.ai.azure.com/api/projects/p", true)]
    public void Endpoint_shape_can_be_checked_offline_without_auth_or_network(string endpoint, bool expected)
    {
        var status = MerlinOpsIntegrationPreflight.Inspect(endpoint, "model-one", "model-two");
        Assert.Equal(expected, status.FoundryEndpointShapeValidated);
        Assert.True(status.ModelPairSpecified);
        Assert.Equal("NOT_CONNECTED", status.Foundry);
    }

    [Fact]
    public void Model_fallback_must_be_distinct_from_primary()
    {
        var status = MerlinOpsIntegrationPreflight.Inspect(
            "https://test.services.ai.azure.com/api/projects/p", "model-one", "MODEL-ONE");
        Assert.False(status.ModelPairSpecified);
    }

    [Fact]
    public async Task Unknown_scenario_is_rejected_without_executing_workflow()
    {
        var engine = new IncidentImprovementEngine(new PolicyEngine(
            Path.Combine(AppContext.BaseDirectory, "guardrails.yaml")));
        await Assert.ThrowsAsync<ArgumentException>(() => engine.CompareAsync("not-a-valid-case"));
    }
}

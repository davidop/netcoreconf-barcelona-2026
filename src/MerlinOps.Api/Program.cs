using System.Text.Json;
using MerlinOps.Api;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls(Environment.GetEnvironmentVariable("MERLINOPS_URL") ?? "http://localhost:5080");
builder.Services.AddSingleton(new PolicyEngine(Path.Combine(AppContext.BaseDirectory, "guardrails.yaml")));
builder.Services.AddSingleton<SimulationEngine>();
builder.Services.AddSingleton(new SqliteRunStore(
    Environment.GetEnvironmentVariable("MERLINOPS_DB")
    ?? Path.Combine(AppContext.BaseDirectory, "merlinops.db")));
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options => options.AddPolicy("LocalSlidev", policy =>
        policy.WithOrigins("http://localhost:3030", "http://127.0.0.1:3030")
            .WithMethods("GET", "POST")
            .WithHeaders("Content-Type")));
}

var app = builder.Build();
app.Use(async (ctx, next) =>
{
    ctx.Response.Headers["X-Content-Type-Options"] = "nosniff";
    ctx.Response.Headers.CacheControl = "no-store";
    await next();
});
if (app.Environment.IsDevelopment()) app.UseCors("LocalSlidev");

app.MapGet("/health", () => Results.Ok(new
{
    ok = true, runtime = "SIMULATED", cloudWritesEnabled = false,
    integrations = "NOT_CONNECTED"
}));
app.MapGet("/api/config", () => Results.Ok(new
{
    mode = "SIMULATED", liveEnabled = false, cloudWritesEnabled = false,
    dataProvenance = "SYNTHETIC", requiresExternalApproval = true
}));
app.MapGet("/api/scenarios", () => Results.Ok(SimulationEngine.Scenarios));

app.MapPost("/api/runs/simulate", async (StartRun request, SimulationEngine engine, SqliteRunStore store, CancellationToken ct) =>
{
    if (request.Scenario is null || !SimulationEngine.Scenarios.Any(s => s.Id == request.Scenario))
        return Results.BadRequest(new { error = "Unknown scenario" });
    var run = engine.Execute(request.Scenario);
    await store.SaveAsync(run, ct);
    return Results.Created($"/api/runs/{run.Id}", run);
});

app.MapGet("/api/runs/{id}", async (string id, SqliteRunStore store, CancellationToken ct) =>
{
    var run = await store.GetAsync(id, ct);
    return run is null ? Results.NotFound() : Results.Ok(run);
});
app.MapGet("/api/runs/{id}/events", async (string id, SqliteRunStore store, CancellationToken ct) =>
{
    var run = await store.GetAsync(id, ct);
    return run is null ? Results.NotFound() : Results.Ok(new { run.Mode, run.Scenario, run.Events });
});
app.MapGet("/api/runs/{id}/stream", async (string id, SqliteRunStore store, HttpContext context) =>
{
    var run = await store.GetAsync(id, context.RequestAborted);
    if (run is null)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        return;
    }

    context.Response.ContentType = "text/event-stream";
    context.Response.Headers["X-Accel-Buffering"] = "no";
    foreach (var missionEvent in run.Events)
    {
        var payload = JsonSerializer.Serialize(new { mode = run.Mode, missionEvent },
            new JsonSerializerOptions(JsonSerializerDefaults.Web));
        await context.Response.WriteAsync($"event: mission\ndata: {payload}\n\n", context.RequestAborted);
        await context.Response.Body.FlushAsync(context.RequestAborted);
        await Task.Delay(300, context.RequestAborted);
    }
    await context.Response.WriteAsync("event: complete\ndata: {}\n\n", context.RequestAborted);
});

app.Run();

public sealed record StartRun(string? Scenario);
public partial class Program { }

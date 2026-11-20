// Local Aspire orchestration only. No Azure resources are provisioned.
var builder = DistributedApplication.CreateBuilder(args);
builder.AddProject<Projects.MerlinOps_Api>("merlinops-api");
builder.Build().Run();

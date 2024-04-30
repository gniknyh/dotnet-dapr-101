var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.mydapr_ApiService>("apiservice");

builder.AddProject<Projects.mydapr_Web>("webfrontend")
    .WithReference(apiService);

builder.Build().Run();

var builder = DistributedApplication.CreateBuilder(args);

// Add API project
var api = builder.AddProject<Projects.CoffeeTracker_Api>("coffeetracker-api")
    .WithExternalHttpEndpoints();

// Add Web project  
var web = builder.AddProject<Projects.CoffeeTracker_Web>("coffeetracker-web")
    .WithReference(api)
    .WithExternalHttpEndpoints();

builder.Build().Run();

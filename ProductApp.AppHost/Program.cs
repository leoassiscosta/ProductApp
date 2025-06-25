var builder = DistributedApplication.CreateBuilder(args);

var config = builder.Configuration;

var dbName = config["Database:Name"]!;
var dbUser = config["Database:User"]!;
var dbPassword = config["Database:Password"]!;
var dbPort = int.Parse(config["Database:Port"]!);

var apiImage = config["Api:Image"]!;
var apiPort = int.Parse(config["Api:Port"]!);

var connectionString =
    $"Host=pg-db;Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";

// Banco de dados
var db = builder.AddContainer("pg-db", "postgres:16-alpine")
    .WithEnvironment("POSTGRES_USER", dbUser)
    .WithEnvironment("POSTGRES_PASSWORD", dbPassword)
    .WithEnvironment("POSTGRES_DB", dbName)
    .WithEndpoint("postgres", endpoint =>
    {
        endpoint.Port = dbPort;
        endpoint.TargetPort = dbPort;
    });

// API container
var apiService = builder.AddContainer("apiservice", apiImage)
    .WithHttpEndpoint(port: apiPort, targetPort: apiPort)
    .WithEnvironment("ConnectionStrings__Postgres", connectionString)
    .WaitFor(db);

// Frontend
builder.AddProject<Projects.ProductApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WaitFor(apiService);

builder.Build().Run();

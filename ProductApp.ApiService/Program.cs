using FastEndpoints;
using FastEndpoints.Swagger;
using ProductApi.Data;
using ProductApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://+:8000");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();
builder.Services.AddSingleton<ProductRepository>();

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseFastEndpoints();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapGet("/", () => "API is running");

var connStr = builder.Configuration.GetConnectionString("Postgres");
await DbInitializer.EnsureDatabaseCreatedAsync(connStr);

app.Run();













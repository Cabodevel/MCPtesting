using McpTesting.API.Extensions;
using MCPtesting.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpoints(typeof(Program).Assembly);

//Add open telemetry
var tracingOtlpEndpoint = builder.Configuration["OTLP_ENDPOINT_URL"];
builder.Services.AddOpenTelemetryConfiguration(builder.Environment.ApplicationName, tracingOtlpEndpoint);

var app = builder.Build();

app.MapPrometheusScrapingEndpoint();

if(app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapEndpoints();

await app.RunAsync();

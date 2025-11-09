using McpTesting.API.Extensions;
using McpTesting.Application.McpTools;
using MCPtesting.Extensions;
using ModelContextProtocol.Server;
using Scalar.AspNetCore;
using System.Collections.Concurrent;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpoints(typeof(Program).Assembly);

builder.Services.ConfigureLocalization();

builder.Services.ConfigureMcpServer();

//Add open telemetry
var tracingOtlpEndpoint = builder.Configuration["OTLP_ENDPOINT_URL"];
builder.Services.AddOpenTelemetryConfiguration(builder.Environment.ApplicationName, tracingOtlpEndpoint);

var app = builder.Build();

app.UseRequestLocalization(new RequestLocalizationOptions
{
    ApplyCurrentCultureToResponseHeaders = true
});

app.MapPrometheusScrapingEndpoint();

if(app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapEndpoints();
// Map MCP with route parameter for tool category filtering
app.MapMcp("/{toolCategory?}");

await app.RunAsync();


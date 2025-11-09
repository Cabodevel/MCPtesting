using MCPtesting.Abstractions;
using MCPtesting.Constants;
using ModelContextProtocol.Client;

namespace McpTesting.API.Endpoints.Mcps;

public class ListTools(ILogger<ListTools> logger) : IEndpoint
{
    private readonly ILogger<ListTools> _logger = logger;

    public void MapEndpoint(IEndpointRouteBuilder app) => 
        app.MapGet("mcp/tools", async () =>
        {
            var clientTransport = new HttpClientTransport(new HttpClientTransportOptions
            {
                Endpoint = new ("http://localhost:5072")
            });

            var client = await McpClient.CreateAsync(clientTransport);

            return await client.ListToolsAsync();
        })
        .WithTags(Tags.McpTools);
}

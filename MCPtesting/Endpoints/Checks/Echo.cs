using MCPtesting.Abstractions;
using MCPtesting.Constants;

namespace MCPtesting.Endpoints.Checks;

public class Echo : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("checks/echo/{word}", async (
            string word) => word)
        .WithTags(Tags.Checks);
}

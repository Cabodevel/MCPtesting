using McpTesting.Application.McpTools;
using ModelContextProtocol.Server;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace McpTesting.API.Extensions;

public static class McpServerExtensions
{
    public static IServiceCollection ConfigureMcpServer(this IServiceCollection services)
    {
        var toolDictionary = new ConcurrentDictionary<string, McpServerTool[]>();
        PopulateToolDictionary(toolDictionary);

        services
            .AddMcpServer()
            .WithStdioServerTransport()
            .WithHttpTransport(options =>
            {
                options.ConfigureSessionOptions = async (httpContext, mcpOptions, cancellationToken) =>
                {
                    // Determine tool category from route parameters
                    var toolCategory = httpContext.Request.RouteValues["toolCategory"]?.ToString()?.ToLower() ?? "all";

                    // Get pre-populated tools for the requested category
                    if(toolDictionary.TryGetValue(toolCategory, out var tools))
                    {
                        mcpOptions.Capabilities = new();
                        mcpOptions.Capabilities.Tools = new();
                        var toolCollection = mcpOptions.ToolCollection = [];

                        foreach(var tool in tools)
                        {
                            toolCollection.Add(tool);
                        }
                    }
                };
            })
            .WithToolsFromAssembly();

        return services;
    }
    public static void PopulateToolDictionary(ConcurrentDictionary<string, McpServerTool[]> toolDictionary)
    {
        var dateTimeTools = GetToolsForType<CurrentDateTimeTool>();
        McpServerTool[] allTools = [.. dateTimeTools];

        toolDictionary.TryAdd("clock", dateTimeTools);
        toolDictionary.TryAdd("all", allTools);
    }

    static McpServerTool[] GetToolsForType<[DynamicallyAccessedMembers(
        DynamicallyAccessedMemberTypes.PublicMethods)] T>()
    {
        var tools = new List<McpServerTool>();
        var toolType = typeof(T);
        var methods = toolType.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.GetCustomAttributes(typeof(McpServerToolAttribute), false).Any());

        foreach(var method in methods)
        {
            try
            {
                var tool = McpServerTool.Create(method, target: null, new McpServerToolCreateOptions());
                tools.Add(tool);
            }
            catch(Exception ex)
            {
                // Log error but continue with other tools
                Console.WriteLine($"Failed to add tool {toolType.Name}.{method.Name}: {ex.Message}");
            }
        }

        return [.. tools];
    }
}

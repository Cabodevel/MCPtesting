using ModelContextProtocol.Server;
using System.ComponentModel;

namespace McpTesting.Application.McpTools;

[McpServerToolType]
public class CurrentDateTimeTool
{
    [McpServerTool, Description("Gets current local date and time based on time zone")]
    public static string Echo([Description("Time zone to get local datetime. Eg: Europe/Madrid")]string timeZoneId) 
        => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(timeZoneId)).ToString("f");
}

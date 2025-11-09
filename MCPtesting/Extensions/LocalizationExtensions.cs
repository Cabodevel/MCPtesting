namespace McpTesting.API.Extensions;

public static class LocalizationExtensions
{
    public static IServiceCollection ConfigureLocalization(this IServiceCollection services)
    {
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[] { "es-es", "en-US" };
            options.SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
        });

        return services;
    }
}

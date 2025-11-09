using MCPtesting.Abstractions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace MCPtesting.Extensions;

public static class AddEndpointsServices
{
    public static IServiceCollection AddEndpoints(
    this IServiceCollection services,
    Assembly assembly)
    {
        var serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }
}

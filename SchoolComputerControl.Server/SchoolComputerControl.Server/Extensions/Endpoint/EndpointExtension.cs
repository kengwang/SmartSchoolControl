using System.Reflection;
using SchoolComputerControl.Server.Interfaces;

namespace SchoolComputerControl.Server.Extensions.Endpoint;

public static class EndpointExtension
{
    public static void AddEndpointsByAssembly(this WebApplicationBuilder builder, params Assembly[] assemblyCollection)
    {
        var allEndpoints = new List<IEndpoint>();
        foreach (var assembly in assemblyCollection)
        {
            foreach (var endpointType in assembly.ExportedTypes
                         .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface))
            {
                if (Activator.CreateInstance(endpointType) is not IEndpoint endpoint) continue;
                allEndpoints.Add(endpoint);
                builder.Services.AddSingleton(typeof(IEndpoint), endpoint);
            }
        }
        
        foreach (var endpoint in allEndpoints)
        {
            endpoint.ConfigureBuilder(builder);
        }
    }

    public static void UseEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
        foreach (var endpoint in endpoints)
        {
            endpoint.ConfigureApp(app);
        }
    }
}
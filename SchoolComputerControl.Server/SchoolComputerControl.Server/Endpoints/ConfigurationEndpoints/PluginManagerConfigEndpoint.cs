using SchoolComputerControl.PluginManager;
using SchoolComputerControl.Server.Interfaces;
using SchoolComputerControl.ServerPluginBase;

namespace SchoolComputerControl.Server.Endpoints.ConfigurationEndpoints;

public class PluginManagerConfigEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddPluginManager<IServerPluginBase>();
    }

    public void ConfigureApp(WebApplication app)
    {
        app.UsePluginManager();
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SchoolComputerControl.ServerPluginBase;

public static class PluginManagerExtension
{

    public static void UsePluginManager(this WebApplication app)
    {
        // First Get All The Plugins
        var pluginBases = app.Services.GetRequiredService<IEnumerable<IServerPluginBase>>();
        foreach (var serverPluginBase in pluginBases)
        {
            serverPluginBase.UseServerPlugin(app);
        }
    }
}
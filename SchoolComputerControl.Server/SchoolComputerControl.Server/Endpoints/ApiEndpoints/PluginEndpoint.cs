using Microsoft.AspNetCore.Mvc;
using SchoolComputerControl.PluginManager;
using SchoolComputerControl.Server.Interfaces;
using SchoolComputerControl.ServerPluginBase;

namespace SchoolComputerControl.Server.Endpoints.ApiEndpoints;

public class PluginEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        // Nothing
    }

    public void ConfigureApp(WebApplication app)
    {
        app.MapGet("/plugins", GetPluginsClient);
        app.MapGet("/plugin/{id}", GetPluginClient);
        app.MapGet("/plugin/{id}/client/download", PluginClientDownload);
    }

    private IResult PluginClientDownload(
        [FromServices] IPluginManager pluginManager,
        [FromServices] IEnumerable<IServerPluginBase> pluginBases,
        [FromRoute] string id)
    {
        if (pluginBases.FirstOrDefault(t => t.Id == id) is { } pluginBase)
        {
            return Results.File($"{pluginManager.PluginRootPath}/{id}/client.zip");
        }
        else
        {
            return BetterResults.NotFound();
        }
    }

    private IResult GetPluginClient([FromServices] IEnumerable<IServerPluginBase> pluginBases, [FromRoute] string id)
    {
        if (pluginBases.FirstOrDefault(t => t.Id == id) is { } pluginBase)
        {
            return BetterResults.Ok(pluginBase.PluginInfo);
        }
        else
        {
            return BetterResults.NotFound();
        }
    }

    private IResult GetPluginsClient([FromServices] IEnumerable<IServerPluginBase> pluginBases)
    {
        return BetterResults.Ok(pluginBases.Select(t => t.PluginInfo));
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolComputerControl.CommunicationPackages.Requests;
using SchoolComputerControl.PluginBase;
using SchoolComputerControl.Server.Endpoints.ConfigurationEndpoints;
using SchoolComputerControl.Server.Interfaces;
using SchoolComputerControl.ServerPluginBase;

namespace SchoolComputerControl.Server.Endpoints.ApiEndpoints;

public class ClientConfigEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        // Nothing
    }

    public void ConfigureApp(WebApplication app)
    {
        app.MapGet("/client/{clientId:guid}/config", GetClientConfig);
        app.MapPost("/client/{clientId:guid}/config/sync", PostClientConfigSync);
        app.MapPost("/client/{clientId:guid}/config", PostClientConfig).AddRouteHandlerFilter<AuthenticationFilter>();
    }

    private static async Task<IResult> PostClientConfig(
        [FromServices] IEnumerable<IServerPluginBase> pluginBases,
        [FromRoute] Guid clientId,
        [FromBody] ClientConfigSyncRequest request,
        [FromServices] ServerDbContext dbContext)
    {
        if (await dbContext.Clients.FirstOrDefaultAsync() is not { } client) return BetterResults.NotFound("客户端未找到");
        foreach (var requestConfig in request.Configs)
        {
            if (client.Configs.FirstOrDefault(t =>
                    t.PluginId == requestConfig.PluginId && t.ConfigId == requestConfig.ConfigId) is
                not { } clientConfig) continue;
            {
                var serverPluginBases = pluginBases.ToList();
                if (serverPluginBases.ToList().FirstOrDefault(t => t.Id == requestConfig.PluginId.ToString())?.Configs
                        .FirstOrDefault(t => t.Id == requestConfig.ConfigId)?.ServerAccessibility ==
                    PluginConfigAccessibility.Editable)
                {
                    clientConfig.Value = requestConfig.Value;
                }
            }
        }

        dbContext.Clients.Update(client);
        return BetterResults.Ok(client.Configs);
    }

    private static async Task<IResult> PostClientConfigSync(
        [FromServices] IEnumerable<IServerPluginBase> pluginBases,
        [FromRoute] Guid clientId,
        [FromBody] ClientConfigSyncRequest request,
        [FromServices] ServerDbContext dbContext)
    {
        if (await dbContext.Clients.FirstOrDefaultAsync() is not { } client) return BetterResults.NotFound("客户端未找到");
        foreach (var requestConfig in request.Configs)
        {
            if (client.Configs.FirstOrDefault(t =>
                    t.PluginId == requestConfig.PluginId && t.ConfigId == requestConfig.ConfigId) is
                not { } clientConfig) continue;
            {
                var serverPluginBases = pluginBases.ToList();
                if (serverPluginBases.ToList().FirstOrDefault(t => t.Id == requestConfig.PluginId.ToString())?.Configs
                        .FirstOrDefault(t => t.Id == requestConfig.ConfigId)?.ClientAccessibility ==
                    PluginConfigAccessibility.Editable)
                {
                    clientConfig.Value = requestConfig.Value;
                }
            }
        }

        dbContext.Clients.Update(client);
        return BetterResults.Ok(client.Configs);
    }

    private static async Task<IResult> GetClientConfig(
        [FromServices] ServerDbContext dbContext,
        [FromRoute] Guid clientId)
    {
        if (await dbContext.Clients.FirstOrDefaultAsync(t => t.Id == clientId) is { } client)
        {
            return BetterResults.Ok(client.Configs);
        }
        else
        {
            return BetterResults.NotFound("客户端未找到");
        }
    }
}
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolComputerControl.CommunicationPackages.Models;
using SchoolComputerControl.CommunicationPackages.Requests;
using SchoolComputerControl.PluginBase;
using SchoolComputerControl.Server.Endpoints.ConfigurationEndpoints;
using SchoolComputerControl.Server.Interfaces;
using SchoolComputerControl.Server.Models.DbModels;
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
        app.MapPost("/client/{clientId:guid}/config/sync", PostClientConfigSync)
            .AddRouteHandlerFilter<ValidationFilter<ClientConfigSyncRequest>>();
        app.MapPost("/client/{clientId:guid}/config", PostClientConfig)
            .AddRouteHandlerFilter<ValidationFilter<ClientConfigSyncRequest>>()
            .AddRouteHandlerFilter<AuthenticationFilter>();
    }

    private static async Task<IResult> PostClientConfig(
        [FromServices] IEnumerable<IServerPluginBase> pluginBases,
        [FromRoute] Guid clientId,
        [FromBody] ClientConfigSyncRequest request,
        [FromServices] ServerDbContext dbContext)
    {
        if (await dbContext.Clients.FirstOrDefaultAsync() is not { } client) return BetterResults.NotFound("客户端未找到");
        var changed = false;
        foreach (var requestConfig in request.Configs)
        {
            var clientConfig = new ClientConfig();
            var dbClientConfig = client.Configs.FirstOrDefault(t =>
                t.PluginId == requestConfig.PluginId && t.ConfigId == requestConfig.ConfigId);
            if (dbClientConfig is { })
            {
                clientConfig = dbClientConfig;
            }

            var serverPluginBases = pluginBases.ToList();
            if (serverPluginBases.FirstOrDefault(t => t.Id == requestConfig.PluginId)?.Configs
                    .FirstOrDefault(t => t.Id == requestConfig.ConfigId)?.ServerAccessibility !=
                PluginConfigAccessibility.Editable) continue;
            changed = true;
            ChangeClientConfig(requestConfig, ref clientConfig, client);
        }

        if (!changed) return BetterResults.Ok(client.Configs);
        dbContext.Clients.Update(client);
        await dbContext.SaveChangesAsync();
        return BetterResults.Ok(client.Configs);
    }

    private static void ChangeClientConfig(ClientConfig requestConfig, ref ClientConfig? clientConfig, Client client)
    {
        // 这段代码好难看, 啊啊啊啊! 有哪个大佬能帮忙改改
        // ! HELP WANTED !
        switch (requestConfig)
        {
            case ClientConfig<string> requestConfigString:
                switch (clientConfig)
                {
                    case ClientConfig<string> clientConfigString:
                        clientConfigString.Value = requestConfigString.Value;
                        break;
                    default:
                        clientConfig = new ClientConfig<string>()
                        {
                            PluginId = requestConfig.PluginId,
                            ConfigId = requestConfig.ConfigId,
                            Value = requestConfigString.Value
                        };
                        client.Configs.Add(clientConfig);
                        break;
                }

                break;
            case ClientConfig<bool> requestConfigBool:
                switch (clientConfig)
                {
                    case null:
                        clientConfig = new ClientConfig<bool>()
                        {
                            PluginId = requestConfig.PluginId,
                            ConfigId = requestConfig.ConfigId,
                            Value = requestConfigBool.Value
                        };
                        client.Configs.Add(clientConfig);
                        break;
                    case ClientConfig<bool> clientConfigString:
                        clientConfigString.Value = clientConfigString.Value;
                        break;
                }

                break;
            case ClientConfig<int> requestConfigInt:
                switch (clientConfig)
                {
                    case null:
                        clientConfig = new ClientConfig<int>()
                        {
                            PluginId = requestConfig.PluginId,
                            ConfigId = requestConfig.ConfigId,
                            Value = requestConfigInt.Value
                        };
                        client.Configs.Add(clientConfig);
                        break;
                    case ClientConfig<int> clientConfigInt:
                        clientConfigInt.Value = requestConfigInt.Value;
                        break;
                }

                break;
            case ClientConfig<DateTime> requestConfigDateTime:
                switch (clientConfig)
                {
                    case null:
                        clientConfig = new ClientConfig<DateTime>()
                        {
                            PluginId = requestConfig.PluginId,
                            ConfigId = requestConfig.ConfigId,
                            Value = requestConfigDateTime.Value
                        };
                        client.Configs.Add(clientConfig);
                        break;
                    case ClientConfig<DateTime> clientConfigDateTime:
                        clientConfigDateTime.Value = requestConfigDateTime.Value;
                        break;
                }

                break;
        }
    }

    private static async Task<IResult> PostClientConfigSync(
        [FromServices] IEnumerable<IServerPluginBase> pluginBases,
        [FromRoute] Guid clientId,
        [FromBody] ClientConfigSyncRequest request,
        [FromServices] ServerDbContext dbContext)
    {
        if (await dbContext.Clients.FirstOrDefaultAsync() is not { } client) return BetterResults.NotFound("客户端未找到");
        var changed = false;
        foreach (var requestConfig in request.Configs)
        {
            if (client.Configs.FirstOrDefault(t =>
                    t.PluginId == requestConfig.PluginId && t.ConfigId == requestConfig.ConfigId) is
                not { } clientConfig) continue;
            {
                var serverPluginBases = pluginBases.ToList();
                if (serverPluginBases.ToList().FirstOrDefault(t => t.Id == requestConfig.PluginId)?.Configs
                        .FirstOrDefault(t => t.Id == requestConfig.ConfigId)?.ClientAccessibility !=
                    PluginConfigAccessibility.Editable)
                    continue;
                changed = true;
                ChangeClientConfig(requestConfig, ref clientConfig, client);
            }
        }

        if (!changed) return BetterResults.Ok(client.Configs);
        dbContext.Clients.Update(client);
        await dbContext.SaveChangesAsync();
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

        return BetterResults.NotFound("客户端未找到");
    }
}

public class ClientConfigSyncRequestValidation : AbstractValidator<ClientConfigSyncRequest>
{
    public ClientConfigSyncRequestValidation()
    {
        RuleFor(req => req.Configs).NotNull();
        RuleForEach(req => req.Configs)
            .SetValidator(new ClientConfigValidation());
    }
}

public class ClientConfigValidation : AbstractValidator<ClientConfig>
{
    public ClientConfigValidation()
    {
        RuleFor(cfg => cfg.PluginId).NotEmpty();
        RuleFor(cfg => cfg.ConfigId).NotEmpty();
    }
}
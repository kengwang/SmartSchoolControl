using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolComputerControl.Infrastructure;
using SchoolComputerControl.Infrastructure.Models;
using SchoolComputerControl.Infrastructure.Requests;
using SchoolComputerControl.Infrastructure.Responses;
using SchoolComputerControl.PluginBase;
using SchoolComputerControl.Server.Endpoints.ConfigurationEndpoints;
using SchoolComputerControl.Server.Interfaces;
using SchoolComputerControl.ServerPluginBase;

namespace SchoolComputerControl.Server.Endpoints.ApiEndpoints;

public class ClientConfigsEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        // Nothing
    }

    public void ConfigureApp(WebApplication app)
    {
        app.MapGet("/client/{clientId:guid}/configs", GetClientConfig);
        app.MapGet("/client/{clientId:guid}/configs/server", GetClientConfigServer)
            .AddRouteHandlerFilter<AuthenticationFilter>();
        app.MapPut("/client/{clientId:guid}/configs", PostClientConfigSync);
        app.MapPut("/client/{clientId:guid}/configs/server", PostClientConfig)
            .AddRouteHandlerFilter<AuthenticationFilter>();
    }

    public static void MergeClientConfigs(
        Dictionary<string, List<ClientConfig>> requestClientAllConfigs,
        List<IServerPluginBase> pluginBases,
        Dictionary<string, List<ClientConfig>> clientDbAllConfigs,
        Func<PluginConfig, PluginConfigAccessibility> accessibilitySelector)
    {
        // 以 Plugin 内的配置信息为基准进行遍历
        foreach (var pluginBase in pluginBases)
        {
            var clientDbConfigs = clientDbAllConfigs.GetValueOrDefault(pluginBase.Id) ?? new List<ClientConfig>();
            var accessiblePluginConfigs = pluginBase.Configs
                .Where(t => accessibilitySelector.Invoke(t) == PluginConfigAccessibility.Editable).ToList();
            if (requestClientAllConfigs.FirstOrDefault(t => t.Key == pluginBase.Id) is var reqClientConfig)
            {
                // 进行同步
                var needUpdates = accessiblePluginConfigs
                    .Select(accConfig => reqClientConfig.Value.FirstOrDefault(t => t.ConfigId == accConfig.ConfigId))
                    .OfType<ClientConfig>()
                    .ToList();
                foreach (var needUpdate in needUpdates)
                {
                    clientDbConfigs.RemoveAll(config => config.ConfigId == needUpdate.ConfigId);
                    clientDbConfigs.Add(needUpdate);
                }
            }

            clientDbAllConfigs[pluginBase.Id] = clientDbConfigs;
        }
    }

    private static async Task<IResult> PostClientConfig(
        [FromServices] IEnumerable<IServerPluginBase> pluginBases,
        [FromRoute] Guid clientId,
        [FromBody] ClientConfigSyncRequest request,
        [FromServices] ServerDbContext dbContext)
    {
        if (await dbContext.Clients.FirstOrDefaultAsync() is not { } client) return BetterResults.NotFound("客户端未找到");
        MergeClientConfigs(request, pluginBases.ToList(), client.Configs,
            config => config.ServerAccessibility);
        dbContext.Clients.Update(client);
        await dbContext.SaveChangesAsync();

        return BetterResults.Ok(client.Configs);
    }

    private static async Task<IResult> PostClientConfigSync(
        [FromServices] IEnumerable<IServerPluginBase> pluginBases,
        [FromRoute] Guid clientId,
        [FromBody] ClientConfigSyncRequest request,
        [FromServices] ServerDbContext dbContext)
    {
        if (await dbContext.Clients.FirstOrDefaultAsync() is not { } client) return BetterResults.NotFound("客户端未找到");
        MergeClientConfigs(request, pluginBases.ToList(), client.Configs,
            config => config.ServerAccessibility);
        dbContext.Clients.Update(client);
        await dbContext.SaveChangesAsync();
        return BetterResults.Ok(client.Configs);
    }

    private static async Task<IResult> GetClientConfig(
        [FromServices] ServerDbContext dbContext,
        [FromServices] IEnumerable<IServerPluginBase> serverPluginBases,
        [FromRoute] Guid clientId)
    {
        if (await dbContext.Clients.FirstOrDefaultAsync(t => t.Id == clientId) is { } client)
        {
            return BetterResults.Ok(SmashClientConfigs(serverPluginBases.ToList(), client.Configs,
                cfg => cfg.ClientAccessibility));
        }

        return BetterResults.NotFound("客户端未找到");
    }

    private static async Task<IResult> GetClientConfigServer(
        [FromServices] ServerDbContext dbContext,
        [FromServices] IEnumerable<IServerPluginBase> serverPluginBases,
        [FromRoute] Guid clientId)
    {
        if (await dbContext.Clients.FirstOrDefaultAsync(t => t.Id == clientId) is { } client)
        {
            return BetterResults.Ok(SmashClientConfigs(serverPluginBases.ToList(), client.Configs,
                cfg => cfg.ServerAccessibility));
        }

        return BetterResults.NotFound("客户端未找到");
    }

    public static Dictionary<string, ClientConfigsResponse> SmashClientConfigs(
        List<IServerPluginBase> pluginBases,
        Dictionary<string, List<ClientConfig>> clientDbAllConfigs,
        Func<PluginConfig, PluginConfigAccessibility> accessibilitySelector)
    {
        // 以 Plugin 内的配置信息为基准进行遍历
        var retConfigs = new Dictionary<string, ClientConfigsResponse>();
        foreach (var pluginBase in pluginBases)
        {
            retConfigs[pluginBase.Id] = new ClientConfigsResponse();
            var clientDbConfigs = clientDbAllConfigs.GetValueOrDefault(pluginBase.Id) ?? new List<ClientConfig>();
            var accessiblePluginConfigs = pluginBase.Configs
                .Where(t => accessibilitySelector.Invoke(t) != PluginConfigAccessibility.Invisible).ToList();
            foreach (var accessiblePluginConfig in accessiblePluginConfigs)
            {
                ClientConfigResponse config;
                if (clientDbConfigs.FirstOrDefault(t => t.ConfigId == accessiblePluginConfig.ConfigId) is { } dbConfig)
                {
                    config = dbConfig switch
                    {
                        ClientConfig<bool> clientConfigBool => new ClientConfigResponse<bool>()
                        {
                            Value = clientConfigBool.Value
                        },
                        ClientConfig<string> clientConfigString => new ClientConfigResponse<string>()
                        {
                            Value = clientConfigString.Value
                        },
                        ClientConfig<DateTime> clientConfigDateTime => new ClientConfigResponse<DateTime>()
                        {
                            Value = clientConfigDateTime.Value
                        },
                        ClientConfig<int> clientConfigInt => new ClientConfigResponse<int>()
                        {
                            Value = clientConfigInt.Value
                        },
                        _ => new ClientConfigResponse()
                    };
                }
                else
                {
                    config = accessiblePluginConfig.DefaultConfig switch
                    {
                        ClientConfig<bool> clientConfigBool => new ClientConfigResponse<bool>()
                        {
                            Value = clientConfigBool.Value
                        },
                        ClientConfig<string> clientConfigString => new ClientConfigResponse<string>()
                        {
                            Value = clientConfigString.Value
                        },
                        ClientConfig<DateTime> clientConfigDateTime => new ClientConfigResponse<DateTime>()
                        {
                            Value = clientConfigDateTime.Value
                        },
                        ClientConfig<int> clientConfigInt => new ClientConfigResponse<int>()
                        {
                            Value = clientConfigInt.Value
                        },
                        _ => new ClientConfigResponse()
                    };
                }

                config.Name = accessiblePluginConfig.Name;
                config.Description = accessiblePluginConfig.Description;
                config.ConfigId = accessiblePluginConfig.ConfigId;
                config.Editable = accessibilitySelector.Invoke(accessiblePluginConfig) ==
                                  PluginConfigAccessibility.Editable;
                retConfigs[pluginBase.Id].Add(config);
            }
        }

        return retConfigs;
    }
}

public class ClientConfigsPostValidation : AbstractValidator<ClientConfigSyncRequest>
{
    public ClientConfigsPostValidation()
    {
        RuleForEach(x => x).SetValidator(new ClientConfigsValidation());
    }
}

public class ClientConfigsValidation : AbstractValidator<KeyValuePair<string, List<ClientConfig>>>
{
    public ClientConfigsValidation() => RuleForEach(t => t.Value).SetValidator(new ClientConfigValidation());
}

public class ClientConfigValidation : AbstractValidator<ClientConfig>
{
    public ClientConfigValidation() => RuleFor(x => x.ConfigId).NotEmpty();
}
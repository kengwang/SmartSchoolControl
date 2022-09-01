using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolComputerControl.Infrastructure.Models.DbModels;
using SchoolComputerControl.Infrastructure.Requests;
using SchoolComputerControl.Infrastructure.Responses;
using SchoolComputerControl.Server.Endpoints.ConfigurationEndpoints;
using SchoolComputerControl.Server.Interfaces;
using SchoolComputerControl.ServerPluginBase;

namespace SchoolComputerControl.Server.Endpoints.ApiEndpoints;

public class ActionEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        // Nothing
    }

    public void ConfigureApp(WebApplication app)
    {
        app.MapGet("/action/{actionId:guid}", GetClientAction);
        app.MapGet("/client/{clientId:guid}/actions", GetClientActions);
        app.MapGet("/action/all", GetActionsAll).AddRouteHandlerFilter<AuthenticationFilter>();
        app.MapGet("/action/available", GetActionsAvailable).AddRouteHandlerFilter<AuthenticationFilter>();
        app.MapPost("/action", AddAction).AddRouteHandlerFilter<AuthenticationFilter>()
            .AddFluentValidationFilter<ActionPostValidation>();
        app.MapPut("/action/{actionId:guid}", UpdateAction).AddFluentValidationFilter<ActionPostValidation>()
            .AddRouteHandlerFilter<AuthenticationFilter>();
    }

    private static async Task<IResult> UpdateAction(Guid actionId, [FromServices] ServerDbContext dbContext,
        ActionPutRequest request)
    {
        if (dbContext.Actions.FirstOrDefault(t => t.Id == actionId) is { } dbAction)
        {
            var clients = await dbContext.Clients.ToListAsync();
            dbAction.Clients = request.ClientIds.Select(id => clients.FirstOrDefault(client => client.Id == id))
                .OfType<Client>()
                .ToList();
            dbAction.StartDateTime = request.StartTime;
            dbAction.EndDateTime = request.EndTime;
            dbAction.Actions = request.Actions;
            dbContext.Actions.Update(dbAction);
            await dbContext.SaveChangesAsync();
            return BetterResults.Ok();
        }

        return BetterResults.NotFound("未找到动作");
    }

    private static async Task<IResult> AddAction([FromServices] ServerDbContext dbContext, ActionPutRequest request)
    {
        var clients = await dbContext.Clients.ToListAsync();
        var action = new ClientAction
        {
            Id = Guid.NewGuid(),
            Clients = request.ClientIds.Select(id => clients.FirstOrDefault(client => client.Id == id)).OfType<Client>()
                .ToList(),
            StartDateTime = request.StartTime,
            EndDateTime = request.EndTime,
            Actions = request.Actions
        };
        dbContext.Actions.Add(action);
        await dbContext.SaveChangesAsync();
        return Results.Created($"/action/{action.Id}", action);
    }

    private static IResult GetActionsAvailable([FromServices] IEnumerable<IServerPluginBase> serverPluginBases)
    {
        var actions = new ActionsResponse();
        var pluginBases = serverPluginBases.ToList();
        foreach (var serverPluginBase in pluginBases)
        {
            actions.AddRange(serverPluginBase.Actions.Select(action => new ActionResponse
            {
                PluginId = serverPluginBase.Id,
                ActionId = action.ActionId,
                Name = action.Name,
                Description = action.Description,
                DefaultValue = action.DefaultValue
            }).ToList());
        }

        return BetterResults.Ok(actions);
    }

    private static async Task<IResult> GetActionsAll(ServerDbContext dbContext)
    {
        return BetterResults.Ok(await dbContext.Actions.ToListAsync());
    }


    private static async Task<IResult> GetClientActions(
        [FromServices] ServerDbContext dbContext,
        [FromRoute] Guid clientId)
    {
        if (!await dbContext.Clients.AnyAsync(t => t.Id == clientId))
            return BetterResults.NotFound("客户端未注册");
        return BetterResults.Ok(await dbContext.Actions
            .Where(t => t.EndDateTime > DateTime.Now && t.Clients.Any(client => client.Id == clientId)).ToListAsync());
    }

    private static async Task<IResult> GetClientAction(
        [FromServices] ServerDbContext dbContext,
        [FromRoute] Guid actionId)
    {
        if (await dbContext.Actions.FirstOrDefaultAsync(t => t.Id == actionId) is { } action)
        {
            return BetterResults.Ok(action);
        }

        return BetterResults.NotFound("执行未找到");
    }
}

public class ActionPostValidation : AbstractValidator<ActionPutRequest>
{
    public ActionPostValidation()
    {
        RuleFor(x => x.Actions).NotEmpty();
        RuleFor(x => x.EndTime).NotEmpty();
        RuleFor(x => x.StartTime).NotEmpty();
        RuleFor(x => x.ClientIds).NotEmpty();
    }
}
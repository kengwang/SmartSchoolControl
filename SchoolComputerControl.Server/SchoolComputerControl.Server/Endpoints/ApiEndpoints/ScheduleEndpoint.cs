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

public class ScheduleEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        // Nothing
    }

    public void ConfigureApp(WebApplication app)
    {
        app.MapGet("/schedule/{scheduleId:guid}", GetSchedule);
        app.MapGet("/client/{clientId:guid}/schedules", GetClientSchedules);
        app.MapGet("/schedule/all", GetSchedulesAll).AddRouteHandlerFilter<AuthenticationFilter>();
        app.MapGet("/actions/available", GetActionsAvailable).AddRouteHandlerFilter<AuthenticationFilter>();
        app.MapPost("/schedule", AddSchedule).AddRouteHandlerFilter<AuthenticationFilter>()
            .AddFluentValidationFilter<Schedule>();
        app.MapPut("/schedule/{scheduleId:guid}", UpdateSchedule).AddFluentValidationFilter<SchedulePostValidation>()
            .AddRouteHandlerFilter<AuthenticationFilter>();
    }

    private static async Task<IResult> UpdateSchedule(Guid scheduleId, [FromServices] ServerDbContext dbContext,
        SchedulePutRequest requestSchedule)
    {
        if (dbContext.Schedules.FirstOrDefault(t => t.Id == scheduleId) is { } dbSchedule)
        {
            var clients = await dbContext.Clients.ToListAsync();
            dbSchedule.Clients = requestSchedule.Clients.Select(id => clients.FirstOrDefault(client => client.Id == id))
                .OfType<Client>()
                .ToList();
            dbSchedule.StartDateTime = requestSchedule.StartDateTime;
            dbSchedule.ExpireDateTime = requestSchedule.ExpireDateTime;
            dbSchedule.Actions = requestSchedule.Actions;
            dbContext.Schedules.Update(dbSchedule);
            await dbContext.SaveChangesAsync();
            return BetterResults.Ok();
        }

        return BetterResults.NotFound("未找到任务");
    }

    private static async Task<IResult> AddSchedule([FromServices] ServerDbContext dbContext, SchedulePutRequest requestSchedule)
    {
        var clients = await dbContext.Clients.ToListAsync();
        var schedule = new Schedule
        {
            Id = Guid.NewGuid(),
            Clients = requestSchedule.Clients.Select(id => clients.FirstOrDefault(client => client.Id == id)).OfType<Client>()
                .ToList(),
            StartDateTime = requestSchedule.StartDateTime,
            ExpireDateTime = requestSchedule.ExpireDateTime,
            Actions = requestSchedule.Actions
        };
        dbContext.Schedules.Add(schedule);
        await dbContext.SaveChangesAsync();
        return Results.Created($"/schedule/{schedule.Id}", schedule);
    }

    private static IResult GetActionsAvailable([FromServices] IEnumerable<IServerPluginBase> serverPluginBases)
    {
        var actions = new ActionsResponse();
        var pluginBases = serverPluginBases.ToList();
        foreach (var serverPluginBase in pluginBases)
        {
            actions.AddRange(serverPluginBase.Actions);
        }

        return BetterResults.Ok(actions);
    }

    private static async Task<IResult> GetSchedulesAll(ServerDbContext dbContext)
    {
        return BetterResults.Ok(await dbContext.Schedules.ToListAsync());
    }


    private static async Task<IResult> GetClientSchedules(
        [FromServices] ServerDbContext dbContext,
        [FromRoute] Guid clientId)
    {
        if (!await dbContext.Clients.AnyAsync(t => t.Id == clientId))
            return BetterResults.NotFound("客户端未注册");
        return BetterResults.Ok(await dbContext.Schedules
            .Where(t => t.ExpireDateTime > DateTime.Now && t.Clients.Any(client => client.Id == clientId)).ToListAsync());
    }

    private static async Task<IResult> GetSchedule(
        [FromServices] ServerDbContext dbContext,
        [FromRoute] Guid scheduleId)
    {
        if (await dbContext.Schedules.FirstOrDefaultAsync(t => t.Id == scheduleId) is { } schedule)
        {
            return BetterResults.Ok(schedule);
        }

        return BetterResults.NotFound("任务未找到");
    }
}

public class SchedulePostValidation : AbstractValidator<SchedulePutRequest>
{
    public SchedulePostValidation()
    {
        RuleFor(x => x.Actions).NotEmpty();
        RuleFor(x => x.Clients).NotEmpty();
        RuleFor(x => x.ExpireDateTime).NotEmpty();
        RuleFor(x => x.StartDateTime).NotEmpty();
    }
}
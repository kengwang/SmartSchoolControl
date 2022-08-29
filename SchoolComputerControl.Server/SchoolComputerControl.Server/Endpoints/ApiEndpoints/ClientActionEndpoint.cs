using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolComputerControl.Server.Endpoints.ConfigurationEndpoints;
using SchoolComputerControl.Server.Interfaces;

namespace SchoolComputerControl.Server.Endpoints.ApiEndpoints;

public class ClientActionEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        // Nothing
    }

    public void ConfigureApp(WebApplication app)
    {
        app.MapGet("/client/action/{actionId:guid}", GetClientAction);
        app.MapGet("/client/{clientId:guid}/actions", GetClientActions);
    }
    
    
    private static async Task<IResult> GetClientActions(
        [FromServices] ServerDbContext dbContext,
        [FromRoute] Guid clientId)
    {
        if (!await dbContext.Clients.AnyAsync(t => t.Id == clientId))
            return BetterResults.NotFound("客户端未注册");
        return BetterResults.Ok(await dbContext.ClientActions
            .Where(t => t.EndDateTime > DateTime.Now && t.Clients.Any(t => t.Id == clientId)).ToListAsync());
    }
    
    private static async Task<IResult> GetClientAction(
        [FromServices] ServerDbContext dbContext,
        [FromRoute] Guid actionId)
    {
        if (await dbContext.ClientActions.FirstOrDefaultAsync(t => t.Id == actionId) is { } action)
        {
            return BetterResults.Ok(action);
        }

        return BetterResults.NotFound("执行未找到");
    }
}
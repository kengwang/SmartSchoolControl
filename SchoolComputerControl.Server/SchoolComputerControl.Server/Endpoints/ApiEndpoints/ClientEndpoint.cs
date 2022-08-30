using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolComputerControl.CommunicationPackages.Models;
using SchoolComputerControl.CommunicationPackages.Requests;
using SchoolComputerControl.Server.Endpoints.ConfigurationEndpoints;
using SchoolComputerControl.Server.Interfaces;
using SchoolComputerControl.Server.Models.DbModels;

namespace SchoolComputerControl.Server.Endpoints.ApiEndpoints;

public class ClientEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IValidator<ClientRegisterRequest>, ClientRegisterValidation>();
    }

    public void ConfigureApp(WebApplication app)
    {
        app.MapPost("/client", ClientRegister)
            .AddRouteHandlerFilter<ValidationFilter<ClientRegisterRequest>>();
        app.MapGet("/client/{clientId:guid}", GetClientById);
        app.MapGet("/clients", GetClients);

    }

    private static async Task<IResult> GetClients([FromServices] ServerDbContext dbContext)
    {
        return BetterResults.Ok(await dbContext.Clients.ToListAsync());
    }

    private static async Task<IResult> GetClientById(
        [FromServices] ServerDbContext dbContext,
        [FromRoute] Guid clientId)
    {
        if (await dbContext.Clients.FirstOrDefaultAsync() is { } client)
        {
            return BetterResults.Ok(client);
        }

        return BetterResults.NotFound("客户端未找到");
    }


    private static async Task<IResult> ClientRegister(
        [FromBody] ClientRegisterRequest request,
        [FromServices] ServerDbContext dbContext)
    {
        var client = new Client
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            LastHeartBeat = DateTime.Now,
            Tags = new List<string>() { request.Name },
            Configs = new List<ClientConfig>()
        };
        await dbContext.Clients.AddAsync(client);
        await dbContext.SaveChangesAsync();
        return Results.Created($"/client/{client.Id}", client);
    }
}

public class ClientRegisterValidation : AbstractValidator<ClientRegisterRequest>
{
    public ClientRegisterValidation()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
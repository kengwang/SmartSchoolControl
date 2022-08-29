using FluentValidation;
using Microsoft.AspNetCore.Mvc;
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
        app.MapPost("/client/register", ClientRegister)
            .AddRouteHandlerFilter<ValidationFilter<ClientRegisterRequest>>();

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
            Tags = new List<string>() { request.Name }
        };
        await dbContext.Clients.AddAsync(client);
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
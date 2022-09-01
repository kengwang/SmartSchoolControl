using SchoolComputerControl.Infrastructure.Models.DbModels;
using SchoolComputerControl.Server.Interfaces;

namespace SchoolComputerControl.Server.Endpoints.ApiEndpoints;

public class EventEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        // Nothing
    }

    public void ConfigureApp(WebApplication app)
    {
        app.MapGet("/action", GetClientEvents);
    }

    private async Task<ClientAction> GetClientEvents(
        )
    {
        throw new NotImplementedException();
    }
}
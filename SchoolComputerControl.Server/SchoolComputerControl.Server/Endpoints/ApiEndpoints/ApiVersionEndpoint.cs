using System.Reflection;
using SchoolComputerControl.Server.Interfaces;

namespace SchoolComputerControl.Server.Endpoints.ApiEndpoints;

public class ApiVersionEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        // Nothing
    }

    public void ConfigureApp(WebApplication app)
    {
        app.MapGet("/version", () => Assembly.GetExecutingAssembly().GetName().Version?.ToString());
    }
}
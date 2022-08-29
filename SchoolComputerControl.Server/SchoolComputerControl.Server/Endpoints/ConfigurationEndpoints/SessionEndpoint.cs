using SchoolComputerControl.Server.Interfaces;

namespace SchoolComputerControl.Server.Endpoints.ConfigurationEndpoints;

public class SessionEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession();
    }

    public void ConfigureApp(WebApplication app)
    {
        app.UseSession();
    }
}
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using SchoolComputerControl.Server.Interfaces;

namespace SchoolComputerControl.Server.Endpoints.ConfigurationEndpoints;

public class JsonConversionEndpoint : IEndpoint
{

    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.Configure<JsonOptions>(option =>
        {
            option.SerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
        });
    }

    public void ConfigureApp(WebApplication app)
    {
        // Nothing
    }
}
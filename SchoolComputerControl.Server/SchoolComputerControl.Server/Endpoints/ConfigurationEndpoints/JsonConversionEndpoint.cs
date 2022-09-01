using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using SchoolComputerControl.Infrastructure.Models;
using SchoolComputerControl.PluginBase;
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

[JsonSerializable(typeof(Dictionary<string,List<ClientConfig>>), GenerationMode = JsonSourceGenerationMode.Default)]
public partial class ClientConfigJsonSerializeContext : JsonSerializerContext
{

}

[JsonSerializable(typeof(List<string>), GenerationMode = JsonSourceGenerationMode.Default)]
public partial class ListStringJsonSerializeContext : JsonSerializerContext
{
    
}
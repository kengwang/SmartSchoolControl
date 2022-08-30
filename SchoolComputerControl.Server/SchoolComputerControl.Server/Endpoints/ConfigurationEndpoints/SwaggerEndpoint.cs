#if DEBUG
using Microsoft.OpenApi.Models;
using SchoolComputerControl.Server.Interfaces;

namespace SchoolComputerControl.Server.Endpoints.ConfigurationEndpoints;

public class SwaggerEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc($"v{typeof(SwaggerEndpoint).Assembly.GetName().Version?.Major ?? 1}", new OpenApiInfo()
            {
                Title = typeof(SwaggerEndpoint).Assembly.GetName().Name,
                Version = $"v{typeof(SwaggerEndpoint).Assembly.GetName().Version?.Major ?? 1}"
            });
        });
    }

    public void ConfigureApp(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
            c.SwaggerEndpoint($"/swagger/v{typeof(SwaggerEndpoint).Assembly.GetName().Version?.Major ?? 1}/swagger.json", typeof(SwaggerEndpoint).Assembly.GetName().Name));
    }
}
#endif
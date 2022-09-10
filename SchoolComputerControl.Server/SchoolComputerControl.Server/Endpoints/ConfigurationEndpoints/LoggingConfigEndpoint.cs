using SchoolComputerControl.Server.Interfaces;
using Serilog;
using Serilog.Events;

namespace SchoolComputerControl.Server.Endpoints.ConfigurationEndpoints;

public class LoggingConfigEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File($"{Environment.CurrentDirectory}/log/log.log",
                rollingInterval: RollingInterval.Day)
            .WriteTo.Console()
            .CreateLogger();
        builder.Logging.ClearProviders();
        builder.Logging.AddEventSourceLogger();
        builder.Logging.AddSerilog();
    }

    public void ConfigureApp(WebApplication app)
    {
        // Nothing
    }
}
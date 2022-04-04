using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Extensions.Logging;
using SmartSchoolControl.Client.Console.Abstractions;
using SmartSchoolControl.Client.Console.Services;
using SmartSchoolControl.Client.Db;
using SmartSchoolControl.Common.Base.Abstractions;

var logConfiguration = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day);

var builder = Host.CreateDefaultBuilder(args);

var host = builder
    .UseSerilog(logConfiguration.CreateLogger())
    .ConfigureServices((_, services) =>
    {
        services.AddDbContext<ClientDbContext>(options =>
        {
            options.UseSqlite("Data Source=client.db",
                sqliteOptions => sqliteOptions.MigrationsAssembly("SmartSchoolControl.Client.Db"));
        });
        services.AddSingleton<IServerConnection, HttpServerConnection>();
        services.AddSingleton<IClientStateManager, ClientStateManager>();
        services.AddSingleton<IPackageFactory, PackageFactory>();
        services.AddSingleton<IPluginManager, PluginManager>();
    })
    .Build();
    
host.Start();
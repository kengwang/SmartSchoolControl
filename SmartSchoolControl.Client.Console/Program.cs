using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using Serilog;
using Serilog.Extensions.Logging;
using SmartSchoolControl.Client.Console.Abstractions;
using SmartSchoolControl.Client.Console.Services;
using SmartSchoolControl.Client.Db;
using SmartSchoolControl.Common.Base.Abstractions;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Requests;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Returns;

var logConfiguration = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day);

var apiSetting = new ApiSetting();
if (File.Exists("apisettings.json"))
{
    try
    {
        apiSetting = JsonSerializer.Deserialize<ApiSetting>(File.ReadAllText("apisettings.json"));
    }
    catch (Exception _)
    {
        await FirstRun();
    }
}
else
{
    await FirstRun();
}


async Task FirstRun()
{
    FirstRun:
    // 首次启动
    Console.WriteLine("检测到首次启动, 请输入服务端地址: ");
    var serveraddr = Console.ReadLine();
    Console.WriteLine("请输入友好名称: ");
    var friendlyName = Console.ReadLine();
    Console.WriteLine("正在请求服务端反馈...");
    var guid = Guid.NewGuid();
    var connection = new HttpServerConnection(new OptionsWrapper<ApiSetting>(new ApiSetting
    {
        ServerApi = serveraddr ?? throw new ArgumentNullException(nameof(serveraddr)),
        ClientId = guid
    }));
    var res = await connection.RequestAsync("Client/Register", new ClientRegisterPackage
    {
        FriendlyName = friendlyName,
        ClientId = guid,
        Name = Environment.MachineName
    });
    if (res?.Status == true)
    {
        File.WriteAllText("apisettings.json", JsonSerializer.Serialize(new ApiSetting
        {
            ServerApi = serveraddr,
            ClientId = guid
        }));
        Console.WriteLine("服务器反馈成功, 已保存设置");
        Console.WriteLine("正在设置开机启动项...");
        Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true)
            ?.SetValue("SmartSchoolControl", Process.GetCurrentProcess().MainModule.FileName);
        Console.WriteLine("设置成功, 请手动重启程序");
        Console.ReadLine();
    }
    else
    {
        Console.WriteLine("服务端反馈失败, 请重试");
        goto FirstRun;
    }
}


var builder = Host.CreateDefaultBuilder(args);
var host = builder
    .UseSerilog(logConfiguration.CreateLogger())
    .ConfigureAppConfiguration((ctx, config) =>
    {
        config.AddJsonFile("apisettings.json");
        config.AddEnvironmentVariables();
        config.AddCommandLine(args);
    })
    .ConfigureServices((_, services) =>
    {
        services.AddOptions();
        services.Configure<ApiSetting>(t => t = apiSetting);
        services.AddDbContext<ClientDbContext>(options =>
        {
            options.UseSqlite("Data Source=client.db",
                sqliteOptions => sqliteOptions.MigrationsAssembly("SmartSchoolControl.Client.Db"));
        });
        services.AddSingleton<IServerConnection, HttpServerConnection>();
        services.AddSingleton<IClientStateManager, ClientStateManager>();
        services.AddSingleton<IPackageFactory, PackageFactory>();
        services.AddSingleton<IPluginManager, PluginManager>();
        services.AddSingleton<ITimerService, TimerService>();
        services.AddSingleton<IClientInfoManager, ClientInfoManager>();
        services.AddSingleton<IHeartBeatService, ServerHeartBeatService>();
    })
    .Build();
var serviceScope = host.Services.CreateScope();
serviceScope.ServiceProvider.GetRequiredService<IClientStateManager>();
serviceScope.ServiceProvider.GetRequiredService<ITimerService>();
host.Run();

public class ApiSetting
{
    public string ServerApi { get; set; }
    public Guid ClientId { get; set; }
}
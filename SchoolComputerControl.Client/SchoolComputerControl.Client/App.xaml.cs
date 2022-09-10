using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SchoolComputerControl.Client.Extensions;
using SchoolComputerControl.Client.Interfaces;
using SchoolComputerControl.Client.Services;
using SchoolComputerControl.ClientPluginBase;
using SchoolComputerControl.PluginManager;

namespace SchoolComputerControl.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        public static IHost? AppHost;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public static IServiceProvider Services { get; set; } = null!;
        
        
        public App()
        {
            var builder = Host.CreateDefaultBuilder();
            builder.ConfigureServices(ConfigureServices);
            builder.ConfigureLogging(ConfigureLogging);
            AppHost = builder.Build();
            Services = AppHost.Services;
        }

        private void ConfigureLogging(ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.AddSerilog(new LoggerConfiguration()
#if DEBUG
                .WriteTo.Console()
#endif
                .WriteTo.File("logs/log.log", rollingInterval: RollingInterval.Day)
                .CreateLogger());
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ClientDbContext>();
            services.AddHttpClient<IHttpRequester, HttpRequester>();
            services.AddPluginManager<IClientPluginBase>();
            services.AddPagesFromAssembly(Assembly.GetExecutingAssembly());
            services.AddViewModelFromAssembly(Assembly.GetExecutingAssembly());
            services.AddWindowsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddServicesFromAssembly(Assembly.GetExecutingAssembly());
            services.AddSingleton(typeof(ISetting<>), typeof(FileJsonSetting<>));
            services.AddSingleton<INotificationPublisher, NotificationPublisher>();
            services.AddNotificationReceiverFromAssembly(Assembly.GetExecutingAssembly());
            services.AddHostedService<HeartBeatService>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Single Instance
            foreach (var process in Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName)
                         .Where(t => t.Id != Environment.ProcessId))
            {
                process.Kill(true);
            }

            await AppHost.RunAsync();
        }
    }
}
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SchoolComputerControl.Client.Extensions;
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

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public static IServiceProvider Services { get; set; } = null!;
        
        
        public App()
        {
            var builder = Host.CreateDefaultBuilder();
            builder.ConfigureServices(ConfigureServices);
            builder.ConfigureLogging(ConfigureLogging);
            var app = builder.Build();
            Services = app.Services;
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
            services.AddPluginManager<IClientPluginBase>();
            services.AddPagesFromAssembly(Assembly.GetExecutingAssembly());
            services.AddViewModelFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Single Instance
            foreach (var process in Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName)
                         .Where(t => t.Id != Environment.ProcessId))
            {
                process.Kill(true);
            }
        }
    }
}
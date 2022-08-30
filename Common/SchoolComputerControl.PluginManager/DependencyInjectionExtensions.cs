using Microsoft.Extensions.DependencyInjection;
using SchoolComputerControl.PluginBase;

namespace SchoolComputerControl.PluginManager;

public static class DependencyInjectionExtensions
{
    public static void AddPluginManager<TImplementation>(this IServiceCollection services, Action<PluginManagerOptions>? option = null) where TImplementation : IPluginBase
    {
        // First Create a new PluginManager Instance
        var pluginManagerOptions = new PluginManagerOptions();
        option?.Invoke(pluginManagerOptions);
        var pluginManager = pluginManagerOptions.CustomPluginManagerActivator == null ? new PluginManager(pluginManagerOptions.PluginRootDirectory!) : pluginManagerOptions.CustomPluginManagerActivator?.Invoke()!;
        services.AddSingleton(typeof(IPluginManager), pluginManager);
        // Load All Plugins
        pluginManager.InitializeAllPlugins<TImplementation>(services);
    }

    public class PluginManagerOptions
    {
        public string? PluginRootDirectory { get; set; } = Environment.CurrentDirectory + "/plugins";
        public Func<IPluginManager>? CustomPluginManagerActivator { get; set; }
    }
}
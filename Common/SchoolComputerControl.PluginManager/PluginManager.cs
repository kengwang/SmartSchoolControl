using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SchoolComputerControl.PluginBase;

namespace SchoolComputerControl.PluginManager;

public class PluginManager : IPluginManager
{
    private readonly string _pluginRootDirectory;
    public PluginManager(string pluginRootDirectory)
    {
        _pluginRootDirectory = pluginRootDirectory;
    }

    public string PluginRootPath => _pluginRootDirectory;

    public void InitializeAllPlugins<TImplementation>(IServiceCollection services) where TImplementation : IPluginBase
    {
        if (!Directory.Exists(_pluginRootDirectory)) return;
        foreach (var pluginDirectory in Directory.EnumerateDirectories(_pluginRootDirectory))
        {
            // Load Assembly
            var pluginId = Path.GetFileName(pluginDirectory);
            var pluginPath = Path.Combine(pluginDirectory, $"{pluginId}.dll");
            var pluginContext = new PluginLoadContext(pluginDirectory);
            var pluginAssembly = pluginContext.LoadFromAssemblyPath(pluginPath);
            
            // Get IPlugin's Implementation
            var pluginTypes = pluginAssembly.GetTypes().Where(type => typeof(TImplementation).IsAssignableFrom(type));
            foreach (var plugin in pluginTypes)
            {
                services.AddSingleton(typeof(TImplementation), plugin);
            }
        }
    }
}
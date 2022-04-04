using System.Reflection;
using System.Text.Json;
using SmartSchoolControl.Common.Base.Abstractions;
using SmartSchoolControl.Server.Backend.Models;

namespace SmartSchoolControl.Server.Backend.Managers;

public class PluginManager : IPluginManager
{
    public List<IPlugin> Plugins { get; } = new();

    public async Task LoadAllPlugins()
    {
        Plugins.Clear();
        var pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
        if (!Directory.Exists(pluginPath)) return;
        var pluginFiles = Directory.GetDirectories(pluginPath);
        foreach (var pluginFile in pluginFiles)
        {
            var plugin = await LoadPlugin(pluginFile);
            if (plugin != null)
            {
                Plugins.Add(plugin);
            }
        }
    }

    private static async Task<ServerPlugin?> LoadPlugin(string pluginFile)
    {
        var pluginJson = await File.ReadAllTextAsync(Path.Combine(pluginFile, "plugin.json"));
        return JsonSerializer.Deserialize<ServerPlugin>(pluginJson);
    }
}
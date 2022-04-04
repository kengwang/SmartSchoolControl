using SmartSchoolControl.Client.Console.Abstractions;
using SmartSchoolControl.Common.Base.Abstractions;

namespace SmartSchoolControl.Client.Console.Services;

public class PluginManager : IPluginManager
{
    public List<IPlugin> Plugins { get; } = new();
    
    public async Task LoadAllPlugins()
    {
        var pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
        foreach (var directory in Directory.GetDirectories(pluginPath))
        {
            
        }
    }
    

    private readonly IClientStateManager _clientStateManager; 
    
    public PluginManager(IClientStateManager clientStateManager)
    {
        _clientStateManager = clientStateManager;
    }
}
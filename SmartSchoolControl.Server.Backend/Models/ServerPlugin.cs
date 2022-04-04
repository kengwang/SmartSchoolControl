using SmartSchoolControl.Common.Base.Abstractions;

namespace SmartSchoolControl.Server.Backend.Models;

public class ServerPlugin : IPlugin
{
    public string Id { get; }
    public string Name { get; }
    public string Description { get; }
    public string Author { get; }
    public string Version { get; }
    public Dictionary<string, PluginAction> Actions { get; }
}


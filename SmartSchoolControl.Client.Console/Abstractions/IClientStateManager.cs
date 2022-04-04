using SmartSchoolControl.Common.Base.Abstractions;
using SmartSchoolControl.Common.Base.DbModels;

namespace SmartSchoolControl.Client.Console.Abstractions;

public interface IClientStateManager
{
    public ClientState ClientState { get; set; }
}

public class ClientState
{
    public Dictionary<string, short>? Permissions { get; set; } = new();
    public List<IPlugin>? Plugins { get; set; } = new();
    public List<ScheduledTask>? ServerTasks { get; set; } = new();
}
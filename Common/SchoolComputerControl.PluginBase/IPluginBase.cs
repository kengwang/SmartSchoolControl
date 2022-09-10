namespace SchoolComputerControl.PluginBase;

public interface IPluginBase
{
    public string Id { get; }
    public PluginInfo PluginInfo { get; }
    public int ApiVersion { get; }
    public List<PluginConfig> Configs { get; }
    public List<PluginAction> Actions { get; }
    public Task HandleEventAsync(string eventId, string param);
}
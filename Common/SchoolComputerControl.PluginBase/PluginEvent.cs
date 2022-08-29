namespace SchoolComputerControl.PluginBase;

public class PluginEvent
{
    public string EventId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string DefaultValue { get; set; }
    public bool CanServerTrigger { get; set; }
    public bool CanClientTrigger { get; set; }
}

namespace SchoolComputerControl.PluginBase;

public class PluginAction
{
    public string ActionId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string DefaultValue { get; set; } = default!;
    public bool CanServerTrigger { get; set; }
    public bool CanClientTrigger { get; set; }
}

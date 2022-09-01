using System.Text.Json.Serialization;

namespace SchoolComputerControl.PluginBase;

public class PluginConfig
{
    public string ConfigId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public PluginConfigAccessibility ServerAccessibility { get; set; } = PluginConfigAccessibility.Invisible;
    public PluginConfigAccessibility ClientAccessibility { get; set; } = PluginConfigAccessibility.Invisible;
    public PluginConfigType Type { get; set; }
    public ClientConfig DefaultConfig { get; set; } = default!;
}


public enum PluginConfigAccessibility
{
    Invisible,
    Visible,
    Editable
}

public enum PluginConfigType
{
    Text, // string
    Boolean, // bool
    Number, // int
    DateTime, // DateTime
}

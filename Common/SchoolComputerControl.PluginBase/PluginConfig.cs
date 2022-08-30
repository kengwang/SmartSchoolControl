using System.Text.Json.Serialization;

namespace SchoolComputerControl.PluginBase;

[JsonDerivedType(typeof(PluginConfig<string>),"string")]
[JsonDerivedType(typeof(PluginConfig<bool>),"bool")]
[JsonDerivedType(typeof(PluginConfig<int>),"number")]
[JsonDerivedType(typeof(PluginConfig<DateTime>),"datetime")]
public class PluginConfig
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public PluginConfigAccessibility ServerAccessibility { get; set; } = PluginConfigAccessibility.Invisible;
    public PluginConfigAccessibility ClientAccessibility { get; set; } = PluginConfigAccessibility.Invisible;
    public PluginConfigType Type { get; set; }
}

public class PluginConfig<TValue> : PluginConfig
{
    public TValue Value = default!;
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

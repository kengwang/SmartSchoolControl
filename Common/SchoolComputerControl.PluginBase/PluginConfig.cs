namespace SchoolComputerControl.PluginBase;

public class PluginConfig
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
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

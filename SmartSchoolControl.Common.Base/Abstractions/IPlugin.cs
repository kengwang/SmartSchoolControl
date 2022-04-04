namespace SmartSchoolControl.Common.Base.Abstractions;

public interface IPlugin
{
    public string Id { get; }
    public string Name { get; }
    public string Description { get; }
    public string Author { get; }
    public string Version { get; }
    public Dictionary<string, PluginAction> Actions { get; }
}

public class PluginAction
{
    public string Id { get; }
    public string Name { get; }
    public string? Description { get; }
    public Permission Permission { get; }
    public List<PluginActionParameter> Parameters { get; }
}

public enum Permission
{
    Anyone,
    ClientAuth,
    ServerAuth,
    None
}

public class PluginActionParameter
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public PluginActionParameterValueType Type { get; set; }
    public string? DefaultValue { get; set; }
}

public enum PluginActionParameterValueType
{
    String,
    Integer,
    Boolean,
    DateTime
}
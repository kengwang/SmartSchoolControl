using System.Text.Json.Serialization;

namespace SchoolComputerControl.CommunicationPackages.Models;

[JsonDerivedType(typeof(ClientConfig<string>),"string")]
[JsonDerivedType(typeof(ClientConfig<bool>),"bool")]
[JsonDerivedType(typeof(ClientConfig<int>),"number")]
[JsonDerivedType(typeof(ClientConfig<DateTime>),"datetime")]
public class ClientConfig
{
    [JsonPropertyName("pluginId")]
    public string PluginId { get; set; } = default!;

    [JsonPropertyName("configId")]
    public string ConfigId { get; set; } = default!;
}

public class ClientConfig<TValue> : ClientConfig
{
    [JsonPropertyName("value")]
    public TValue Value { get; set; } = default!;
}
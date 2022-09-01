using System.Text.Json.Serialization;

namespace SchoolComputerControl.PluginBase;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type", IgnoreUnrecognizedTypeDiscriminators = true,
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType)]
[JsonDerivedType(typeof(ClientConfig<string>), "string")]
[JsonDerivedType(typeof(ClientConfig<bool>), "bool")]
[JsonDerivedType(typeof(ClientConfig<int>), "number")]
[JsonDerivedType(typeof(ClientConfig<DateTime>), "datetime")]
public class ClientConfig
{
    public string ConfigId { get; set; } = default!;
}

public class ClientConfig<TValue> : ClientConfig
{
    public TValue Value { get; set; } = default!;
}
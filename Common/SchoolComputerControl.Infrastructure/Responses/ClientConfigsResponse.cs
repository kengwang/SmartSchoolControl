using System.Text.Json.Serialization;
using SchoolComputerControl.PluginBase;

namespace SchoolComputerControl.Infrastructure.Responses;

public class ClientConfigsResponse : List<ClientConfigResponse>
{
}


[JsonPolymorphic(TypeDiscriminatorPropertyName = "type", IgnoreUnrecognizedTypeDiscriminators = true,
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType)]
[JsonDerivedType(typeof(ClientConfigResponse<string>), "string")]
[JsonDerivedType(typeof(ClientConfigResponse<bool>), "bool")]
[JsonDerivedType(typeof(ClientConfigResponse<int>), "number")]
[JsonDerivedType(typeof(ClientConfigResponse<DateTime>), "datetime")]
public class ClientConfigResponse
{
    public string ConfigId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool Editable { get; set; } = false;
}

public class ClientConfigResponse<T> : ClientConfigResponse
{
    public T Value { get; set; } = default!;
}
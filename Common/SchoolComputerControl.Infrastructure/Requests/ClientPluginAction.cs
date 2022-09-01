using System.Text.Json.Serialization;

namespace SchoolComputerControl.Infrastructure.Requests;

public class ClientPluginAction
{
    [JsonPropertyName("id")] public string Id { get; set; } = default!;
    [JsonPropertyName("actionName")] public string ActionName { get; set; } = default!;
    [JsonPropertyName("actionParameter")] public string? ActionParameter { get; set; }
}
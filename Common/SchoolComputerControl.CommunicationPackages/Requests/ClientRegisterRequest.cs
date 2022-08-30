using System.Text.Json.Serialization;

namespace SchoolComputerControl.CommunicationPackages.Requests;

public class ClientRegisterRequest
{
    [JsonPropertyName("name")] public string Name { get; set; } = default!;
}
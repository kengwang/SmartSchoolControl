using System.Text.Json.Serialization;
using SchoolComputerControl.CommunicationPackages.Models;

namespace SchoolComputerControl.CommunicationPackages.Requests;

public class PostClientConfigRequest
{
    [JsonPropertyName("configs")]
    public List<ClientConfig> Configs { get; set; } = default!;
}
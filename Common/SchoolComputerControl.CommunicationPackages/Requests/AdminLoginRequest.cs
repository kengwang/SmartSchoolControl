using System.Text.Json.Serialization;

namespace SchoolComputerControl.CommunicationPackages.Requests;

public class AdminLoginRequest
{
    [JsonPropertyName("username")] public string UserName { get; set; } = null!;

    [JsonPropertyName("password")] public string Password { get; set; } = null!;
}
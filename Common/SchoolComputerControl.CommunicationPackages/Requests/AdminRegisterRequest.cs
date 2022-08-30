using System.Text.Json.Serialization;

namespace SchoolComputerControl.CommunicationPackages.Requests;

public class AdminRegisterRequest
{
    [JsonPropertyName("username")] public string UserName { get; set; } = default!;

    [JsonPropertyName("password")] public string Password { get; set; } = default!;
    [JsonPropertyName("email")] public string Email { get; set; } = default!;
}
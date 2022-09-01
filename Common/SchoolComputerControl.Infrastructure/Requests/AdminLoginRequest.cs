using System.Text.Json.Serialization;

namespace SchoolComputerControl.Infrastructure.Requests;

public class AdminLoginRequest
{
    [JsonPropertyName("username")] public string UserName { get; set; } = default!;

    public string Password { get; set; } = default!;
}
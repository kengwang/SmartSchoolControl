using System.Text.Json.Serialization;

namespace SchoolComputerControl.Infrastructure.Requests;

public class AdminRegisterRequest
{
    [JsonPropertyName("username")] public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Email { get; set; } = default!;
}
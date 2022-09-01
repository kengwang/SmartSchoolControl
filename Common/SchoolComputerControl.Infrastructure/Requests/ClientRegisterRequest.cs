using System.Text.Json.Serialization;

namespace SchoolComputerControl.Infrastructure.Requests;

public class ClientRegisterRequest
{
    public string Name { get; set; } = default!;
}
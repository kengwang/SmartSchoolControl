using System.Text.Json.Serialization;

namespace SchoolComputerControl.Infrastructure.Requests;

public class ClientPluginAction
{
    public string Id { get; set; } = default!;
    public string ActionName { get; set; } = default!;
    public string? ActionParameter { get; set; }
}
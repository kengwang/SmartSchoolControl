namespace SchoolComputerControl.CommunicationPackages.Models;

public class ClientConfig
{
    public Guid Id { get; set; }
    public Guid PluginId { get; set; }
    public string ConfigId { get; set; } = null!;
    public string? Value { get; set; }
}
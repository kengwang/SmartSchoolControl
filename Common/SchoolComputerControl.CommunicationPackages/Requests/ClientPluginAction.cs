namespace SchoolComputerControl.CommunicationPackages.Models;

public class ClientPluginAction
{
    public string Id { get; set; } = null!;
    public string ActionName { get; set; } = null!;
    public string? ActionParameter { get; set; }
}
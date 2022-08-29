using SchoolComputerControl.CommunicationPackages.Models;

namespace SchoolComputerControl.CommunicationPackages.Requests;

public class ClientConfigSyncRequest
{
    public List<ClientConfig> Configs { get; set; } = null!;
}
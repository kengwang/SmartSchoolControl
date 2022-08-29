using SchoolComputerControl.CommunicationPackages.Models;

namespace SchoolComputerControl.CommunicationPackages.Requests;

public class PostClientConfigRequest
{
    public List<ClientConfig> Configs { get; set; } = null!;
}
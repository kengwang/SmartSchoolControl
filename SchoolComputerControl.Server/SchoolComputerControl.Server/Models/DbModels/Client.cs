using SchoolComputerControl.CommunicationPackages.Models;

namespace SchoolComputerControl.Server.Models.DbModels;

public class Client
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTime LastHeartBeat { get; set; }
    public List<string> Tags { get; set; } = default!;

    public List<ClientConfig> Configs { get; set; } = default!;
}
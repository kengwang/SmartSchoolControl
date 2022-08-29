using SchoolComputerControl.CommunicationPackages.Models;

namespace SchoolComputerControl.Server.Models.DbModels;

public class ClientAction
{
    public Guid Id { get; set; }
    public List<Client> Clients { get; set; } = null!;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public List<ClientPluginAction> Actions { get; set; } = null!;
}
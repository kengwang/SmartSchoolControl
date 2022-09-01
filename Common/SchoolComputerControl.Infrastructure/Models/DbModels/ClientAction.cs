using SchoolComputerControl.Infrastructure.Requests;

namespace SchoolComputerControl.Infrastructure.Models.DbModels;

public class ClientAction
{
    public Guid Id { get; set; }
    public List<Client> Clients { get; set; } = default!;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public Dictionary<string,Dictionary<string,string>> Actions { get; set; } = default!;
}
namespace SchoolComputerControl.Infrastructure.Requests;

public class ActionPutRequest
{
    public Dictionary<string,Dictionary<string,string>> Actions { get; set; } = default!;
    public List<Guid> ClientIds { get; set; } = default!;
    public DateTime StartTime { get; set; } = default!;
    public DateTime EndTime { get; set; } = default!;
}
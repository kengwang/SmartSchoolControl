using SchoolComputerControl.Infrastructure.Models.DbModels;
using SchoolComputerControl.Infrastructure.Models.DbModels.Base;

namespace SchoolComputerControl.Infrastructure.Requests;

public class SchedulePutRequest
{
    public Dictionary<string, KeyValuePair<string, string>> Actions { get; set; } = default!;
    public DateTime StartDateTime { get; set; }
    public DateTime ExpireDateTime { get; set; }
    public List<Guid> Clients { get; set; } = default!;
    public bool Enabled { get; set; }
    public List<Trigger> Triggers { get; set; } = default!;
}
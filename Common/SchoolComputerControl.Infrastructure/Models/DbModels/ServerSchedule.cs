using System.Text.Json.Serialization;
using SchoolComputerControl.Infrastructure.Models.DbModels.Base;

namespace SchoolComputerControl.Infrastructure.Models.DbModels;

public class ServerSchedule : ScheduleBase
{
    public List<Client> Clients { get; set; } = default!;
}
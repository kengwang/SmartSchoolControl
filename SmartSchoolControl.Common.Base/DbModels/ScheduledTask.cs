using System.ComponentModel.DataAnnotations;

namespace SmartSchoolControl.Common.Base.DbModels;

public class ScheduledTask
{
    [Key] public Guid Id { get; set; }
    [Required] public string Name { set; get; } = "未命名任务";
    public string? Description { get; set; }
    public bool Enabled { get; set; }
    public List<TaskTrigger> Triggers { get; set; } = new();
    public Workflow? Workflow { get; set; }
    public List<Client> Clients { get; set; } = new();
    public Dictionary<Guid,DateTime> ClientsExecutionTimes { get; set; } = new();
}
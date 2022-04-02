using System.ComponentModel.DataAnnotations;

namespace Server.Base.DbModels;

public class ScheduledTask
{
    [Key] public Guid Id { get; set; }
    [Required] public string Name { set; get; }
    public string Description { get; set; }
    public List<TaskTrigger> Trigger { get; set; }
}
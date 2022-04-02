using System.ComponentModel.DataAnnotations;

namespace Server.Base.DbModels;

public class TaskTrigger
{
    [Key] public Guid Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public TaskTriggerType Type { get; set; }
    public List<TimeSpan>? TimesInDay { get; set; }
    public List<int>? DayInWeek { get; set; }
    public List<DateOnly>? Dates { get; set; }
    public List<DateTime>? DateTimes { get; set; }
    public DateTime? ExecutionOnceTime { get; set; }
}

public enum TaskTriggerType : short
{
    EveryDay,
    EveryWeek,
    MultipleDays,
    StartUp,
    OneTime,
    Special,
}
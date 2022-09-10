using System.Text.Json.Serialization;

namespace SchoolComputerControl.Infrastructure.Models.DbModels.Base;

public class ScheduleBase
{
    public Guid Id { get; set; }
    public Dictionary<string, KeyValuePair<string, string>> Actions { get; set; } = default!;
    public DateTime StartDateTime { get; set; }
    public DateTime ExpireDateTime { get; set; }
    public bool Enabled { get; set; }
    public List<Trigger> Triggers { get; set; } = default!;
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(OnceTrigger), "once")]
[JsonDerivedType(typeof(HourlyTrigger), "hourly")]
[JsonDerivedType(typeof(MinutelyTrigger), "minutely")]
[JsonDerivedType(typeof(DailyTrigger), "daily")]
[JsonDerivedType(typeof(WeeklyTrigger), "weekly")]
[JsonDerivedType(typeof(MonthlyTrigger), "monthly")]
[JsonDerivedType(typeof(YearlyTrigger), "yearly")]
public class Trigger
{
    // Empty Base
}

public class YearlyTrigger : Trigger
{
    public DateOnly Date { get; set; } = default!;
}

public class MonthlyTrigger : Trigger
{
    public List<int> DaysInMonth { get; set; } = default!;
}

public class WeeklyTrigger : Trigger
{
    public List<int> DaysInWeek { get; set; } = default!;
}

public class DailyTrigger : Trigger
{
    public int DaySpan { get; set; }
}

public class MinutelyTrigger : Trigger
{
    public int MinuteSpan { get; set; }
}

public class HourlyTrigger : Trigger
{
    public int HourSpan { get; set; }
}

public class OnceTrigger : Trigger
{
}
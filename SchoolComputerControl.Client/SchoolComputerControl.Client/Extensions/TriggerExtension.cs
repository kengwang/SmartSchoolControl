using System.Collections.Generic;
using System.Linq;
using SchoolComputerControl.Infrastructure.Models.DbModels.Base;

namespace SchoolComputerControl.Client.Extensions;

public static class TriggerExtension
{
    public static bool CanFire(
        this Trigger trigger,
        DateTime firingTime,
        DateTime startTime,
        DateTime expireTime,
        DateTime lastFiringTime)
    {
        if (expireTime <= firingTime) return false;
        if (startTime > firingTime) return false;
        if (lastFiringTime == default) return true;
        return trigger switch
        {
            DailyTrigger dailyTrigger => firingTime <= lastFiringTime.AddDays(dailyTrigger.DaySpan),
            HourlyTrigger hourlyTrigger => firingTime <= lastFiringTime.AddHours(hourlyTrigger.HourSpan),
            MinutelyTrigger minutelyTrigger => firingTime <= lastFiringTime.AddMinutes(minutelyTrigger.MinuteSpan),
            MonthlyTrigger monthlyTrigger => firingTime.Date != lastFiringTime.Date // 在今日未执行过
                                             && monthlyTrigger.DaysInMonth.Contains(firingTime.Month) // 在今月需要执行
                                             && firingTime.TimeOfDay <= startTime.TimeOfDay, // 在今应当执行的每天时间中未执行
            OnceTrigger => true,
            WeeklyTrigger weeklyTrigger => firingTime.Date != lastFiringTime.Date // 在今日未执行过
                                           && weeklyTrigger.DaysInWeek.Contains((int)firingTime.DayOfWeek) // 在今日需要执行
                                           && firingTime.TimeOfDay <= startTime.TimeOfDay, // 今日已经达到执行时间
            YearlyTrigger yearlyTrigger => firingTime.Date != lastFiringTime.Date &&
                                           lastFiringTime.Month == yearlyTrigger.Date.Month &&
                                           lastFiringTime.Day == yearlyTrigger.Date.Day,
            _ => throw new ArgumentOutOfRangeException(nameof(trigger))
        };
    }

    public static bool CanFire(
        this List<Trigger> triggers,
        DateTime firingTime,
        DateTime startTime,
        DateTime expireTime,
        DateTime lastFiringTime
    )
    {
        return triggers.Any(trigger => trigger.CanFire(firingTime, startTime, expireTime, lastFiringTime));
    }
}
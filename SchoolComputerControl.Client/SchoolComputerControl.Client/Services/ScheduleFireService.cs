using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolComputerControl.Client.Extensions;
using SchoolComputerControl.Client.Interfaces;
using SchoolComputerControl.Client.Models;
using SchoolComputerControl.Client.Models.Settings;
using SchoolComputerControl.ClientPluginBase;

namespace SchoolComputerControl.Client.Services;

public class ScheduleFireService : INotificationReceiver<SecondlyBeatNotification>
{
    private readonly List<IClientPluginBase> _plugins;
    private readonly ISetting<FiredTasksSettings> _firedTasksSettings;
    private readonly ClientDbContext _dbContext;

    public ScheduleFireService(IEnumerable<IClientPluginBase> plugins, ISetting<FiredTasksSettings> firedTasksSettings,
        ClientDbContext dbContext)
    {
        _firedTasksSettings = firedTasksSettings;
        _dbContext = dbContext;
        _plugins = plugins.ToList();
    }

    public async Task HandleNotificationAsync(SecondlyBeatNotification notification,
        CancellationToken cancellationToken = default)
    {
        var schedules = await _dbContext.Schedules.ToListAsync(cancellationToken: cancellationToken);
        foreach (var schedule in schedules)
        {
            DateTime lastFireDatetime = default!;
            if (_firedTasksSettings.Setting!.GetValueOrDefault(schedule.Id) is var dateTime)
            {
                lastFireDatetime = dateTime;
            }

            if (!schedule.Enabled || !schedule.Triggers.CanFire(notification.FiredDateTime, schedule.StartDateTime,
                    schedule.ExpireDateTime, lastFireDatetime)) continue;
            foreach (var (pluginId, (eventId, parameter)) in schedule.Actions)
            {
                if (_plugins.FirstOrDefault(plugin => plugin.Id == pluginId) is { } clientPlugin)
                {
                    await clientPlugin.HandleEventAsync(eventId, parameter);
                }
            }

            _firedTasksSettings.Setting![schedule.Id] = notification.FiredDateTime;
        }
    }
}
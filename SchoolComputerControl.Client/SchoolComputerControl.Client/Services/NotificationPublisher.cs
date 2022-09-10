using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SchoolComputerControl.Client.Interfaces;

namespace SchoolComputerControl.Client.Services;

public class NotificationPublisher : INotificationPublisher
{
    private readonly IServiceProvider _services;

    public NotificationPublisher(IServiceProvider services)
    {
        _services = services;
    }

    public async Task PublishAsync<TNotification>(TNotification notification,
        CancellationToken cancellationToken = default)
    {
        var receivers = _services.GetService<IEnumerable<INotificationReceiver<TNotification>>>();
        if (receivers is null) return;
        foreach (var receiver in receivers.ToList())
        {
            await receiver.HandleNotificationAsync(notification, cancellationToken);
        }
    }
}
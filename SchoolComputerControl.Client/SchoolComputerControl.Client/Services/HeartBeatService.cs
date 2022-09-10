using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SchoolComputerControl.Client.Interfaces;
using SchoolComputerControl.Client.Models;

namespace SchoolComputerControl.Client.Services;

public class HeartBeatService : BackgroundService
{
    private readonly PeriodicTimer _timer;
    private readonly INotificationPublisher _publisher;

    public HeartBeatService(INotificationPublisher publisher)
    {
        _publisher = publisher;
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            await _publisher.PublishAsync<SecondlyBeatNotification>(new() { FiredDateTime = DateTime.Now },
                cancellationToken: stoppingToken);
        }
    }
}
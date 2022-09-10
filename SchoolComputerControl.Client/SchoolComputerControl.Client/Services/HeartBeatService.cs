using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SchoolComputerControl.Client.Services;

public class HeartBeatService : BackgroundService
{
    private readonly PeriodicTimer _timer;

    public HeartBeatService()
    {
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            
        }
    }
}
using SmartSchoolControl.Client.Console.Abstractions;

namespace SmartSchoolControl.Client.Console.Services;

public class TimerService : ITimerService
{
    private readonly IHeartBeatService _heartBeatService;
    private readonly Timer _timer;
    public TimerService(IHeartBeatService heartBeatService)
    {
        _heartBeatService = heartBeatService;
        _timer = new Timer(TimerTicked,null,TimeSpan.FromSeconds(5),TimeSpan.FromSeconds(5));
    }

    public void TimerTicked(object? _)
    {
        _heartBeatService.HeatBeat();
    }
}
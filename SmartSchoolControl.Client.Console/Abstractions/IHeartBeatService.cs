namespace SmartSchoolControl.Client.Console.Abstractions;

public interface IHeartBeatService
{
    public Task HeatBeat();
    public void StopHearBeat();
    public void ResumeHearBeat();
}
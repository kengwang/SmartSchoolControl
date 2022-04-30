using SmartSchoolControl.Client.Console.Abstractions;

namespace SmartSchoolControl.Client.Console.Services;

public class ClientInfoManager : IClientInfoManager
{
    private readonly Dictionary<string, string> _clientInfo = new();
    public void AddClientInfo(string display, string value)
    {
        _clientInfo[display] = value;
    }

    public void UpdateClientInfo(string display, string value)
    {
        _clientInfo[display] = value;
    }

    public void RemoveClientInfo(string display)
    {
        _clientInfo.Remove(display);
    }

    public Dictionary<string, string> GetClientInfos()
    {
        return _clientInfo;
    }
}
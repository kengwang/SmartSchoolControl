using SmartSchoolControl.Client.Console.Abstractions;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Requests;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Returns;

namespace SmartSchoolControl.Client.Console.Services;

public class ServerHeartBeatService : IHeartBeatService
{
    private readonly IServerConnection _serverConnection;
    private readonly IPackageFactory _packageFactory;
    private readonly IClientInfoManager _clientInfoManager;
    private bool _stopBeating;

    public ServerHeartBeatService(IServerConnection serverConnection,IPackageFactory packageFactory, IClientInfoManager clientInfoManager)
    {
        _serverConnection = serverConnection;
        _packageFactory = packageFactory;
        _clientInfoManager = clientInfoManager;
    }

    public async Task HeatBeat()
    {
        if (_stopBeating) return;
        await _serverConnection.RequestAsync("Client/HeartBeat", _packageFactory.AppendClientId(new ClientHeartBeatPackage
        {
            ClientInfos = _clientInfoManager.GetClientInfos().Select(t=>new ClientInfo
            {
                Display = t.Key,
                Value = t.Value
            }).ToList()
        }));
    }

    public void StopHearBeat()
    {
        _stopBeating = true;
    }

    public void ResumeHearBeat()
    {
        _stopBeating = false;
    }
}
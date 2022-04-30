using SmartSchoolControl.Client.Console.Abstractions;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Requests;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Returns;

namespace SmartSchoolControl.Client.Console.Services;

public class ClientStateManager : IClientStateManager
{
    private readonly IServerConnection _serverConnection;
    private readonly IPackageFactory _packageFactory;

    public ClientStateManager(IServerConnection serverConnection, IPackageFactory packageFactory)
    {
        _serverConnection = serverConnection;
        _packageFactory = packageFactory;
        LoadClientState();
    }

    public async void LoadClientState()
    {
        var response = await _serverConnection.RequestAsync<ServerOnlinePackage, ClientOnlinePackage>("Client/Online",
            _packageFactory.AppendClientId(new ClientOnlinePackage()));
        if (response?.Status == true)
        {
            ClientState = new ClientState
            {
                Permissions = response.Data?.Permissions ?? default,
                Plugins = response.Data?.Plugins ?? default,
                ServerTasks = response.Data?.Tasks ?? default
            };
        }
    }

    public ClientState? ClientState { get; set; }
}
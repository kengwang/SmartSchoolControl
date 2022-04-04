using Microsoft.Extensions.Configuration;
using SmartSchoolControl.Client.Console.Abstractions;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Requests;

namespace SmartSchoolControl.Client.Console.Services;

public class PackageFactory : IPackageFactory
{
    private Guid _clientId; 
    public PackageFactory(IConfiguration configuration)
    {
        _clientId = configuration.GetValue<Guid>("ClientId");
    }

    public TPackage AppendClientId<TPackage>(TPackage package) where TPackage : ClientPackageBase
    {
        package.ClientId = _clientId;
        return package;
    }

    public void SetClientId(Guid clientId)
    {
        _clientId = clientId;
    }
}
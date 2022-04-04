using SmartSchoolControl.Common.Base.ApiModels.Packages.Requests;

namespace SmartSchoolControl.Client.Console.Abstractions;

public interface IPackageFactory
{
    TPackage AppendClientId<TPackage>(TPackage package) where TPackage : ClientPackageBase;
    void SetClientId(Guid clientId);
}
using SmartSchoolControl.Common.Base.ApiModels.Packages.Requests;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Returns;

namespace SmartSchoolControl.Client.Console.Abstractions;

public interface IServerConnection
{
    public Task<bool> EstablishConnection();
    public Task CloseConnection();

    public Task<TResponseModel?> RequestAsync<TResponseModel, TPackage>(string endpoint,
        ClientPackageBase requestPackage)
        where TResponseModel : ServerReturnModel<TPackage> where TPackage : class;

    public Task<TResponseModel?> RequestAsync<TResponseModel, TPackage>(string endpoint,
        Dictionary<string, string> query)
        where TResponseModel : ServerReturnModel<TPackage> where TPackage : class;
}
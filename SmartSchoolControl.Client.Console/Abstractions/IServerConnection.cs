using SmartSchoolControl.Common.Base.ApiModels.Packages.Requests;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Returns;

namespace SmartSchoolControl.Client.Console.Abstractions;

public interface IServerConnection
{
    public Task<bool> EstablishConnection();
    public Task CloseConnection();

    public Task<ServerReturnModel<TResponseModel>?> RequestAsync<TResponseModel, TRequestPackage>(string endpoint,
        TRequestPackage requestPackage)
        where TResponseModel : class where TRequestPackage : ClientPackageBase;

    public Task<ServerReturnModel<TResponseModel>?> RequestAsync<TResponseModel>(string endpoint,
        Dictionary<string, string> query) where TResponseModel : class;

    public Task<ServerReturnModel?> RequestAsync<TRequestPackage>(string endpoint, TRequestPackage requestPackage)
        where TRequestPackage : ClientPackageBase;

    public Task<ServerReturnModel?> RequestAsync(string endpoint, Dictionary<string, string> query);
}
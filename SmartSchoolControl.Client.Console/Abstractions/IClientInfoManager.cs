using SmartSchoolControl.Common.Base.ApiModels.Packages.Requests;

namespace SmartSchoolControl.Client.Console.Abstractions;

public interface IClientInfoManager
{
    public void AddClientInfo(string display,string value);
    public void UpdateClientInfo(string display,string value);
    public void RemoveClientInfo(string display);
    public Dictionary<string,string> GetClientInfos();
}
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SmartSchoolControl.Client.Console.Abstractions;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Requests;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Returns;

namespace SmartSchoolControl.Client.Console.Services;

public class HttpServerConnection : IServerConnection
{
    private readonly HttpClient _httpClient = new();

    public HttpServerConnection(IOptions<ApiSetting>? apiSetting)
    {
        _httpClient.BaseAddress = new Uri(apiSetting?.Value.ServerApi ?? "http://localhost:5000");
    }

    public Task<bool> EstablishConnection()
    {
        return Task.FromResult(true);
    }

    public Task CloseConnection()
    {
        return Task.CompletedTask;
    }

    public async Task<ServerReturnModel<TResponseModel>?> RequestAsync<TResponseModel, TRequestPackage>(string endpoint,
        TRequestPackage requestPackage)
        where TResponseModel : class where TRequestPackage : ClientPackageBase
    {
        var response = await _httpClient.PostAsJsonAsync(endpoint, requestPackage);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ServerReturnModel<TResponseModel>?>();
    }

    public async Task<ServerReturnModel<TResponseModel>?> RequestAsync<TResponseModel>(string endpoint,
        Dictionary<string, string> query) where TResponseModel : class
    {
        var requestUri = endpoint;
        if (query.Count != 0)
            requestUri += "?" + string.Join('&', query.Select(x => $"{x.Key}={x.Value}"));
        var response = await _httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ServerReturnModel<TResponseModel>?>();
    }

    public async Task<ServerReturnModel?> RequestAsync<TRequestPackage>(string endpoint, TRequestPackage requestPackage)
        where TRequestPackage : ClientPackageBase
    {
        var response = await _httpClient.PostAsJsonAsync(endpoint, requestPackage);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ServerReturnModel>();
    }

    public async Task<ServerReturnModel?> RequestAsync(string endpoint, Dictionary<string, string> query)
    {
        var requestUri = endpoint;
        if (query.Count != 0)
            requestUri += "?" + string.Join('&', query.Select(x => $"{x.Key}={x.Value}"));
        var response = await _httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ServerReturnModel>();
    }
}
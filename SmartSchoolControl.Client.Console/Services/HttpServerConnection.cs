using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using SmartSchoolControl.Client.Console.Abstractions;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Requests;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Returns;

namespace SmartSchoolControl.Client.Console.Services;

public class HttpServerConnection : IServerConnection
{
    private readonly HttpClient _httpClient = new();

    public HttpServerConnection(IConfiguration configuration)
    {
        _httpClient.BaseAddress = new Uri(configuration.GetValue<string>("ApiServerAddress", "http://localhost:5000"));
    }

    public Task<bool> EstablishConnection()
    {
        return Task.FromResult(true);
    }

    public Task CloseConnection()
    {
        return Task.CompletedTask;
    }

    public async Task<TResponseModel?> RequestAsync<TResponseModel,TPackage>(string endpoint, ClientPackageBase requestPackage)
        where TResponseModel : ServerReturnModel<TPackage> where TPackage : class
    {
        var response = await _httpClient.PostAsJsonAsync(endpoint, requestPackage);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponseModel>();
    }

    public async Task<TResponseModel?> RequestAsync<TResponseModel,TPackage>(string endpoint, Dictionary<string, string> query)
        where TResponseModel : ServerReturnModel<TPackage> where TPackage : class
    {
        var requestUri = endpoint;
        if (query.Count != 0)
            requestUri += "?" + string.Join('&', query.Select(x => $"{x.Key}={x.Value}"));
        var response = await _httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponseModel>();
    }
}
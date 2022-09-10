using System.Net.Http;
using SchoolComputerControl.Client.Interfaces;

namespace SchoolComputerControl.Client.Services;

public class HttpRequester : IHttpRequester
{
    public HttpClient Client { get; set; }

    public HttpRequester(HttpClient client)
    {
        Client = client;
    }
}
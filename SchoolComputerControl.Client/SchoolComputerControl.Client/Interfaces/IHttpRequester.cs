using System.Net.Http;
using System.Threading.Tasks;

namespace SchoolComputerControl.Client.Interfaces;

public interface IHttpRequester
{
    public HttpClient Client { get; set; }
}
using System.Text.Json.Serialization;
using SchoolComputerControl.Infrastructure.Models;
using SchoolComputerControl.PluginBase;

namespace SchoolComputerControl.Infrastructure.Requests;

public class ClientConfigSyncRequest : Dictionary<string,List<ClientConfig>>
{
}
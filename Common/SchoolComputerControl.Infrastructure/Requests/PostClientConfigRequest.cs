using System.Text.Json.Serialization;
using SchoolComputerControl.Infrastructure.Models;
using SchoolComputerControl.PluginBase;

namespace SchoolComputerControl.Infrastructure.Requests;

public class PostClientConfigRequest
{
    public List<ClientConfig> Configs { get; set; } = default!;
}
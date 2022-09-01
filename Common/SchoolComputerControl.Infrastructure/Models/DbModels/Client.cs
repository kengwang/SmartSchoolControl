using System.Text.Json.Serialization;
using SchoolComputerControl.PluginBase;

namespace SchoolComputerControl.Infrastructure.Models.DbModels;

public class Client
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTime LastHeartBeat { get; set; }
    public List<string> Tags { get; set; } = default!;

    [JsonIgnore]
    public Dictionary<string,List<ClientConfig>> Configs { get; set; } = default!;
}
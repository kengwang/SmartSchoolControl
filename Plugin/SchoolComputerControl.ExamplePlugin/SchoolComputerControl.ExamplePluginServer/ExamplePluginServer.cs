using Microsoft.AspNetCore.Builder;
using SchoolComputerControl.PluginBase;
using SchoolComputerControl.ServerPluginBase;

namespace SchoolComputerControl.ExamplePluginServer;

public class ExamplePluginServer : IServerPluginBase
{
    public string Id => "kengwang.SchoolComputerControl.ExamplePlugin";

    public PluginInfo PluginInfo => new()
    {
        Id = Id,
        Name = "示例插件",
        Author = "kengwang",
        Description = "这是一个示例插件",
    };

    public int ApiVersion => 1;

    public List<PluginConfig> Configs => new()
    {
        new PluginConfig<string>
        {
            Id = "E00D0E9B-9CC4-451E-BA25-535EDEEF2AA8",
            Name = "文本配置项",
            Description = "一个文本类型配置项",
            ServerAccessibility = PluginConfigAccessibility.Editable,
            ClientAccessibility = PluginConfigAccessibility.Visible,
            Type = PluginConfigType.Text,
            Value = "默认值"
        },
        new PluginConfig<bool>
        {
            Id = "93D5F656-E0B5-4220-9777-55D86C98D5F2",
            Name = "布尔配置项",
            Description = "一个布尔类型的配置项, 不可触动",
            ServerAccessibility = PluginConfigAccessibility.Visible,
            ClientAccessibility = PluginConfigAccessibility.Editable,
            Type = PluginConfigType.Boolean,
            Value = false
        },
        new PluginConfig<int>()
        {
            Id = "089D6800-DA6E-4D20-814D-154303AB0C15",
            Name = "数值类型项",
            Description = "一个数值类型项",
            ServerAccessibility = PluginConfigAccessibility.Editable,
            ClientAccessibility = PluginConfigAccessibility.Invisible,
            Type = PluginConfigType.Number,
            Value = 69
        },
        new PluginConfig<DateTime>
        {
            Id = "AF9D34D8-526A-46ED-B17D-5F2C7225ED88",
            Name = "时间型配置项",
            Description = "懒得写了",
            ServerAccessibility = PluginConfigAccessibility.Editable,
            ClientAccessibility = PluginConfigAccessibility.Editable,
            Type = PluginConfigType.DateTime,
            Value = DateTime.Today
        }
    };

    public List<PluginEvent> Events => new()
    {
        new PluginEvent
        {
            EventId = "ConsoleLog",
            Name = "控制台输出",
            Description = "输出内容到控制台",
            DefaultValue = "Nothing",
            CanServerTrigger = true,
            CanClientTrigger = false
        }
    };


    public Task HandleEvent(string eventId, string param)
    {
        return Task.CompletedTask;
    }

    public void UseServerPlugin(WebApplication app)
    {
        app.MapGet(string.Concat("/plugin/", Id, "/hello"), () => $"Hello From {Id}");
    }
}
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
        new PluginConfig()
        {
            ConfigId = "textConfig",
            Name = "文本配置项",
            Description = "一个文本类型配置项",
            ServerAccessibility = PluginConfigAccessibility.Editable,
            ClientAccessibility = PluginConfigAccessibility.Visible,
            Type = PluginConfigType.Text,
            DefaultConfig = new ClientConfig<string>
            {
                ConfigId = "textConfig",
                Value = "defaultValue"
            }
        },
        new PluginConfig()
        {
            ConfigId = "boolConfig",
            Name = "布尔配置项",
            Description = "一个布尔类型的配置项, 不可触动",
            ServerAccessibility = PluginConfigAccessibility.Visible,
            ClientAccessibility = PluginConfigAccessibility.Editable,
            Type = PluginConfigType.Boolean,
            DefaultConfig = new ClientConfig<bool>
            {
                ConfigId = "boolConfig",
                Value = false
            }
        },
        new PluginConfig()
        {
            ConfigId = "intConfig",
            Name = "数值类型项",
            Description = "一个数值类型项",
            ServerAccessibility = PluginConfigAccessibility.Editable,
            ClientAccessibility = PluginConfigAccessibility.Invisible,
            Type = PluginConfigType.Number,
            DefaultConfig = new ClientConfig<int>
            {
                ConfigId = "intConfig",
                Value = 9000
            }
        },
        new PluginConfig()
        {
            ConfigId = "dateTimeConfig",
            Name = "时间型配置项",
            Description = "懒得写了",
            ServerAccessibility = PluginConfigAccessibility.Editable,
            ClientAccessibility = PluginConfigAccessibility.Editable,
            Type = PluginConfigType.DateTime,
            DefaultConfig = new ClientConfig<DateTime>
            {
                ConfigId = "dateTimeConfig",
                Value = DateTime.Today
            }
        }
    };

    public List<PluginAction> Actions => new()
    {
        new()
        {
            ActionId = "ConsoleLog",
            Name = "控制台输出",
            Description = "输出内容到控制台",
            DefaultValue = "Nothing",
            CanServerTrigger = true,
            CanClientTrigger = false
        }
    };
    


    public Task HandleEventAsync(string eventId, string param)
    {
        return Task.CompletedTask;
    }

    public void UseServerPlugin(WebApplication app)
    {
        app.MapGet(string.Concat("/plugin/", Id, "/hello"), () => $"Hello From {Id}");
    }
}
using Microsoft.AspNetCore.Builder;
using SchoolComputerControl.PluginBase;

namespace SchoolComputerControl.ServerPluginBase;

public interface IServerPluginBase : IPluginBase
{
    public void UseServerPlugin(WebApplication app);
}
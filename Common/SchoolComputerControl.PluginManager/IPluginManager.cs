using Microsoft.Extensions.DependencyInjection;
using SchoolComputerControl.PluginBase;

namespace SchoolComputerControl.PluginManager;

public interface IPluginManager
{
    public string PluginRootPath { get; }
    public void InitializeAllPlugins<TImplementation>(IServiceCollection services) where TImplementation : IPluginBase;
}
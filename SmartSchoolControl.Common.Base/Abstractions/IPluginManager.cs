namespace SmartSchoolControl.Common.Base.Abstractions;

public interface IPluginManager
{
    public List<IPlugin> Plugins { get; }
    public Task LoadAllPlugins();
}
namespace SmartSchoolControl.Common.Base.DbModels;

public class TaskAction
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "未命名动作";
    public string PluginId { get; set; } = "SmartSchoolControl.Plugin.None";
    public Dictionary<string, string> Parameters { get; set; } = new();
    public string Description { get; set; } = "未描述动作";
}
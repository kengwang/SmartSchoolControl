using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Requests;

namespace SmartSchoolControl.Common.Base.DbModels;

public class Client
{
    [Key] public Guid Id { get; set; }

    [Required] public string Name { get; set; } = "主机";

    [Required] public string FriendlyName { get; set; } = "未命名主机";

    [Required] public DateTime LastHeartBeatTime { get; set; }

    public List<ModifiedAssociation> ModifiedAssociations { get; set; } = new();
    
    public Dictionary<string,short> Permissions { get; set; } = new();
    
    public List<ScheduledTask> Tasks { get; set; } = new();
    
    [NotMapped] public bool IsOnline => (DateTime.Now - LastHeartBeatTime).TotalMinutes < 1;
    public List<ClientInfo> ClientInfos { get; set; } = new();
    
    public List<ClientLogging> Loggings { get; set; } = new();
}

public class ModifiedAssociation
{
    public ModifiedAssociationType Type { get; set; }
    public ModifiedAssociationObjectType ObjectType { get; set; }
    public Guid ObjectId { get; set; }
}

public class ClientLogging
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public string Source { get; set; }
}

public enum ModifiedAssociationType
{
    Add,
    Remove,
    Update
}

public enum ModifiedAssociationObjectType
{
    Task,
    Workflow,
    TaskAction,
    TaskTrigger
}
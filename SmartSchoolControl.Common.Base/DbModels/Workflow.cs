using System.ComponentModel.DataAnnotations;

namespace SmartSchoolControl.Common.Base.DbModels;

public class Workflow
{
    public Guid Id { get; set; }
    [Required] public List<TaskAction> TaskActions { get; set; } = new();
    [Required] public WorkflowExecutionFailedAction WorkflowExecutionFailedAction { get; set; }
}

public enum WorkflowExecutionFailedAction : short
{
    Continue,
    Stop,
    Retry
}
using SchoolComputerControl.Infrastructure.Models.DbModels;
using SchoolComputerControl.Infrastructure.Requests;

namespace SchoolComputerControl.Infrastructure.Responses;

public class ActionsResponse : List<ActionResponse>
{
    
}

public class ActionResponse
{
    public string PluginId { get; set; } = default!;
    public string ActionId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string DefaultValue { get; set; } = default!;
}
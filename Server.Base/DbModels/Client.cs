using System.ComponentModel.DataAnnotations;

namespace Server.Base.DbModels;

public class Client
{
    [Key] public Guid Id { get; set; }

    [Required] public string Name { get; set; }

    [Required] public string IP { get; set; }

    [Required] public string FriendlyName { get; set; }

    [Required] public DateTime LastHeartBeatTime { get; set; }

    public bool IsOnline => (DateTime.Now - LastHeartBeatTime).TotalMinutes > 1;
}
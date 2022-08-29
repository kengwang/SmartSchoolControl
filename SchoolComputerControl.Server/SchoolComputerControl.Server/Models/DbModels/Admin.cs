namespace SchoolComputerControl.Server.Models.DbModels;

public class Admin
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool Enable { get; set; } = false;
}
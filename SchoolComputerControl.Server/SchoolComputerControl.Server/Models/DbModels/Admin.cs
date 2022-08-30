namespace SchoolComputerControl.Server.Models.DbModels;

public class Admin
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool Enable { get; set; } = false;
}
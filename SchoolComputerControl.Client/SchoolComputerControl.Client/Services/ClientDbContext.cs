using Microsoft.EntityFrameworkCore;
using SchoolComputerControl.Infrastructure.Models.DbModels;

namespace SchoolComputerControl.Client.Services;

public class ClientDbContext : DbContext
{

    public DbSet<ClientSchedule> Schedules { get; set; } = default!;

    public ClientDbContext(DbContextOptions<ClientDbContext> options) : base(options)
    {
        
    }
}
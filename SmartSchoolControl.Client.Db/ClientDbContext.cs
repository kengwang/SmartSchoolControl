using Microsoft.EntityFrameworkCore;

namespace SmartSchoolControl.Client.Db;

public class ClientDbContext : DbContext
{
    public ClientDbContext(DbContextOptions options) : base(options)
    {
    }
}
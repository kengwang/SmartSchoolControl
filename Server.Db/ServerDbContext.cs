using Microsoft.EntityFrameworkCore;
using Server.Base.DbModels;

namespace Server.Db;

public class ServerDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }

    public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options)
    {
    }
}
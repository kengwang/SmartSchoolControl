using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolComputerControl.Server.Interfaces;
using SchoolComputerControl.Server.Models.DbModels;

namespace SchoolComputerControl.Server.Endpoints.ConfigurationEndpoints;

public class DbContextEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ServerDbContext>(options => { options.UseSqlite("Data Source=database.db"); });
    }

    public void ConfigureApp(WebApplication app)
    {
        // Nothing
    }
}

public class ServerDbContext : DbContext
{
    public DbSet<Admin> Admins { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<ClientAction> ClientActions { get; set; } = null!;

    public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var splitStringConverter =
            new ValueConverter<List<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }).ToList());
        modelBuilder.Entity<Client>()
            .Property(client => client.Tags)
            .HasConversion(splitStringConverter);
    }
}
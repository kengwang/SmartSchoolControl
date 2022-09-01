﻿using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolComputerControl.Infrastructure.Models.DbModels;
using SchoolComputerControl.PluginBase;
using SchoolComputerControl.Server.Interfaces;

namespace SchoolComputerControl.Server.Endpoints.ConfigurationEndpoints;

public class DbContextEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ServerDbContext>(options =>
        {
            options.UseSqlite(
                $"Data Source={Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/database.db");
        });
    }

    public void ConfigureApp(WebApplication app)
    {
        // Nothing
    }
}

public class ServerDbContext : DbContext
{
    public DbSet<Admin> Admins { get; set; } = default!;
    public DbSet<Client> Clients { get; set; } = default!;
    public DbSet<ClientAction> Actions { get; set; } = default!;

    public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var splitStringConverter =
            new ValueConverter<List<string>, string>(
                model => JsonSerializer.Serialize(model, ListStringJsonSerializeContext.Default.ListString),
                v => JsonSerializer.Deserialize(v, ListStringJsonSerializeContext.Default.ListString)!
            );
        modelBuilder.Entity<Client>()
            .Property(client => client.Tags)
            .HasConversion(splitStringConverter);

        var clientConfigJsonConverter = new ValueConverter<Dictionary<string,List<ClientConfig>>, string>(
            model => JsonSerializer.Serialize(model, ClientConfigJsonSerializeContext.Default.DictionaryStringListClientConfig),
            dataString =>
                JsonSerializer.Deserialize(dataString, ClientConfigJsonSerializeContext.Default.DictionaryStringListClientConfig)!
        );

        modelBuilder.Entity<Client>()
            .Property(client => client.Configs)
            .HasConversion(clientConfigJsonConverter);

        var clientActionsConverter = new ValueConverter<Dictionary<string, Dictionary<string, string>>, string>(
            model => JsonSerializer.Serialize(model,
                ClientActionSerializeContext.Default.DictionaryStringDictionaryStringString),
            dbString => JsonSerializer.Deserialize(dbString,
                ClientActionSerializeContext.Default.DictionaryStringDictionaryStringString)!
        );
        modelBuilder.Entity<ClientAction>()
            .Property(action => action.Actions)
            .HasConversion(clientActionsConverter);
    }
}
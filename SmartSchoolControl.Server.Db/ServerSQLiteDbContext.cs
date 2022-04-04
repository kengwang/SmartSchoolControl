using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SmartSchoolControl.Common.Base.DbModels;

namespace SmartSchoolControl.Server.Db;

public class ServerSqLiteDbContext : DbContext
{
    public DbSet<Client>? Clients { get; set; }
    public DbSet<ScheduledTask>? Tasks { get; set; }
    public DbSet<TaskTrigger>? TaskTriggers { get; set; }
    public DbSet<Workflow>? Workflows { get; set; }
    public DbSet<TaskAction>? TaskActions { get; set; }

    public ServerSqLiteDbContext(DbContextOptions<ServerSqLiteDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TaskAction>().Property(t => t.Parameters).HasConversion(
            v => DictionaryConverter.ConvertToString(v),
            v => DictionaryConverter.ConvertFromString<string, string>(v) ?? new());
        modelBuilder.Entity<Client>().Property(t => t.Permissions).HasConversion(
            v => DictionaryConverter.ConvertToString(v),
            v => DictionaryConverter.ConvertFromString<string, short>(v) ?? new());
        modelBuilder.Entity<TaskTrigger>().Property(t => t.Dates).HasConversion(v => ListConverter.ConvertToString(v),
            v => ListConverter.ConvertFromString<DateOnly>(v) ?? new());
        modelBuilder.Entity<TaskTrigger>().Property(t => t.DateTimes).HasConversion(
            v => ListConverter.ConvertToString(v),
            v => ListConverter.ConvertFromString<DateTime>(v) ?? new());
        modelBuilder.Entity<TaskTrigger>().Property(t => t.TimesInDay).HasConversion(
            v => ListConverter.ConvertToString(v),
            v => ListConverter.ConvertFromString<TimeOnly>(v) ?? new());
        modelBuilder.Entity<TaskTrigger>().Property(t => t.DayInWeek).HasConversion(
            v => ListConverter.ConvertToString(v),
            v => ListConverter.ConvertFromString<int>(v) ?? new());
        modelBuilder.Entity<Client>().Property(t => t.ModifiedAssociations).HasConversion(
            v => ListConverter.ConvertToString(v),
            v => ListConverter.ConvertFromString<ModifiedAssociation>(v) ?? new());
        modelBuilder.Entity<ScheduledTask>().Property(t => t.ClientsExecutionTimes).HasConversion(
            v => DictionaryConverter.ConvertToString(v),
            v => DictionaryConverter.ConvertFromString<Guid, DateTime>(v) ?? new());
    }

    private static class DictionaryConverter
    {
        public static Dictionary<TKey, TValue>? ConvertFromString<TKey, TValue>(string value) where TKey : notnull
        {
            try
            {
                return JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(value);
            }
            catch (Exception _)
            {
                return null;
            }
        }

        public static string ConvertToString<TKey, TValue>(Dictionary<TKey, TValue> dictionary) where TKey : notnull
        {
            return JsonSerializer.Serialize(dictionary);
        }
    }

    private static class ListConverter
    {
        public static List<T>? ConvertFromString<T>(string value) where T : notnull
        {
            try
            {
                return JsonSerializer.Deserialize<List<T>>(value);
            }
            catch (Exception _)
            {
                return null;
            }
        }

        public static string ConvertToString<T>(List<T>? list) where T : notnull
        {
            return JsonSerializer.Serialize(list);
        }
    }
}
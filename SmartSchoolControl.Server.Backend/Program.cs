using Microsoft.EntityFrameworkCore;
using Serilog;
using SmartSchoolControl.Common.Base.Abstractions;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Returns;
using SmartSchoolControl.Server.Backend.Managers;
using SmartSchoolControl.Server.Backend.Models;
using SmartSchoolControl.Server.Db;
using SmartSchoolControl.Server.Db.Repositories;
using IPluginManager = SmartSchoolControl.Common.Base.Abstractions.IPluginManager;

var builder = WebApplication.CreateBuilder(args);

// Add the Serilog logger
builder.Logging.ClearProviders();
var logConfiguration = new LoggerConfiguration().WriteTo.Console().CreateLogger();
builder.Services.AddLogging(b => b.AddSerilog(logConfiguration));

// Add the database
builder.Services.AddDbContext<ServerSqLiteDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLite"),
        sqliteOptions => sqliteOptions.MigrationsAssembly("SmartSchoolControl.Server.Db"))
);

// Add repositories
builder.Services.AddScoped(typeof(IRepository<,>), typeof(DbRepository<,>));

// Add Managers
builder.Services.AddSingleton<IPluginManager, PluginManager>(_ =>
{
    var pluginManager = new PluginManager();
    pluginManager.LoadAllPlugins().Wait();
    return pluginManager;
});

// Configure Data Validation Returns
builder.Services.AddControllers().ConfigureApiBehaviorOptions(apiBehaviorOptions =>
{
    apiBehaviorOptions.InvalidModelStateResponseFactory = context =>
        new ServerReturnBase<object>(new ServerReturnModel<object>(false, "Incomplete parameters", -400,
            builder.Environment.IsDevelopment() ? context.ModelState : null), 400);
});
builder.Services.AddCors(t=>t.AddPolicy("AllowAll", p=>p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Server.Backend.Models.Packages.Returns;
using Server.Base.Abstractions;
using Server.Db;
using Server.Db.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add the Serilog logger
builder.Logging.ClearProviders();
var logConfiguration = new LoggerConfiguration().WriteTo.Console().CreateLogger();
builder.Services.AddLogging(b => b.AddSerilog(logConfiguration));

// Add the database
builder.Services.AddDbContext<ServerDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLite"),
        sqliteOptions => sqliteOptions.MigrationsAssembly("Server.Db"))
);

// Add repositories
builder.Services.AddSingleton(typeof(IRepository<,>), typeof(DbRepository<,>));

// Add User Login
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddUserManager<UserManager<IdentityUser>>();

// Configure Data Validation Returns
builder.Services.AddControllers().ConfigureApiBehaviorOptions(apiBehaviorOptions =>
{
    apiBehaviorOptions.InvalidModelStateResponseFactory = context =>
        new ServerReturnBase(false, "Incomplete parameters", -400,
            builder.Environment.IsDevelopment() ? context.ModelState : null, 400);
});

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

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
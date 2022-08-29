using System.Reflection;
using SchoolComputerControl.Server.Extensions.Endpoint;

var builder = WebApplication.CreateBuilder();
builder.AddEndpointsByAssembly(Assembly.GetExecutingAssembly());
var app = builder.Build();
app.UseExceptionHandler("/exception");
app.UseEndpoints();
app.Run();
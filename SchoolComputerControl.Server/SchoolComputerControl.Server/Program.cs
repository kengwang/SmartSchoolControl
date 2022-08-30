using System.Diagnostics;
using System.Reflection;
using SchoolComputerControl.Server.Extensions.Endpoint;
using Serilog;

var stopwatch = Stopwatch.StartNew();
var builder = WebApplication.CreateBuilder();
builder.AddEndpointsByAssembly(Assembly.GetExecutingAssembly());
var app = builder.Build();
app.UseExceptionHandler("/exception");
app.UseEndpoints();
stopwatch.Stop();
Log.Information("[Startup] 预加载完毕, 耗时 {TimeInMillisecond} 毫秒,击败了全国 {Percentage}% 的电脑", stopwatch.ElapsedMilliseconds,
    Math.Max(0, (5000 - stopwatch.ElapsedMilliseconds) / 50));
app.Run();
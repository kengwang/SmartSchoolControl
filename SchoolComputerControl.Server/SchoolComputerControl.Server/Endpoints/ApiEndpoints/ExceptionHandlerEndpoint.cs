using SchoolComputerControl.Server.Interfaces;

namespace SchoolComputerControl.Server.Endpoints.ApiEndpoints;

public class ExceptionHandlerEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        // Nothing
    }

    public void ConfigureApp(WebApplication app)
    {
        app.Map("/exception", ExceptionHandle);
    }

    private IResult ExceptionHandle(HttpContext context)
    {
        return BetterResults.Error("发生内部错误", 500);
    }
}
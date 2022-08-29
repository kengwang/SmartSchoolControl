using System.Diagnostics.CodeAnalysis;
using SchoolComputerControl.Server.Interfaces;

namespace SchoolComputerControl.Server.Endpoints.ConfigurationEndpoints;

public static class AdminLoginRepository
{
    public static readonly Dictionary<string, string> AdminLoginKeys = new();
}

public class AuthenticationFilter : IRouteHandlerFilter
{
    public async ValueTask<object?> InvokeAsync(RouteHandlerInvocationContext context, RouteHandlerFilterDelegate next)
    {
        if (AdminLoginRepository.AdminLoginKeys!.GetValueOrDefault(
                context.HttpContext.Session.GetString("LoginUserId")) !=
            context.HttpContext.Session.GetString("LoginUserKey"))
        {
            return BetterResults.Error("管理员未授权", StatusCodes.Status401Unauthorized);
        }

        return await next(context);
    }
}
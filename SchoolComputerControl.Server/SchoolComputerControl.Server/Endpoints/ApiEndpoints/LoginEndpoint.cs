using CryptoBase.Digests.SHA256;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolComputerControl.CommunicationPackages.Requests;
using SchoolComputerControl.Server.Endpoints.ConfigurationEndpoints;
using SchoolComputerControl.Server.Interfaces;

namespace SchoolComputerControl.Server.Endpoints.ApiEndpoints;

public class LoginEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IValidator<AdminLoginRequest>, AdminLoginValidation>();
    }

    public void ConfigureApp(WebApplication app)
    {
        app.MapPost("/admin/login", AdminLogin).AddRouteHandlerFilter<ValidationFilter<AdminLoginRequest>>();
    }

    private async Task<IResult> AdminLogin(
        [FromServices] ServerDbContext dbContext,
        [FromServices] IEasyHasher<DefaultSHA256Digest> hasher,
        [FromBody] AdminLoginRequest adminLoginRequest,
        HttpContext context)
    {
        if (await dbContext.Admins.FirstOrDefaultAsync(a => a.UserName == adminLoginRequest.UserName) is not { } admin)
            return BetterResults.NotFound("用户不存在");
        if (hasher.GetHashedString(adminLoginRequest.Password) != admin.Password)
            return BetterResults.Error("密码错误", StatusCodes.Status401Unauthorized);
        if (!admin.Enable)
            return BetterResults.Error("用户被禁用", StatusCodes.Status403Forbidden);
        context.Session.SetString("LoginUserId", admin.Id.ToString());
        var loginKey = Guid.NewGuid().ToString("N");
        AdminLoginRepository.AdminLoginKeys[admin.Id.ToString()] = loginKey;
        context.Session.SetString("LoginUserKey", loginKey);
        return BetterResults.Ok(admin);
    }
}

public class AdminLoginValidation : AbstractValidator<AdminLoginRequest>
{
    public AdminLoginValidation()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
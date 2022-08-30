using CryptoBase.Digests.SHA256;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolComputerControl.CommunicationPackages.Requests;
using SchoolComputerControl.Server.Endpoints.ConfigurationEndpoints;
using SchoolComputerControl.Server.Interfaces;
using SchoolComputerControl.Server.Models.DbModels;

namespace SchoolComputerControl.Server.Endpoints.ApiEndpoints;

public class AdminEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        // Nothing
    }

    public void ConfigureApp(WebApplication app)
    {
        app.MapPost("/admin/login", AdminLogin).AddRouteHandlerFilter<ValidationFilter<AdminLoginRequest>>();
        app.MapPost("/admin/register", AdminRegister)
            .AddRouteHandlerFilter<AuthenticationFilter>()
            .AddRouteHandlerFilter<ValidationFilter<AdminRegisterRequest>>();
        app.MapGet("/admin/{adminId:guid}", GetAdmin);
    }

    private static async Task<IResult> GetAdmin([FromServices] ServerDbContext dbContext,
        [FromRoute] Guid adminId)
    {
        if (await dbContext.Admins.FirstOrDefaultAsync(t => t.Id == adminId) is { } admin)
            return BetterResults.Ok(admin);
        return BetterResults.NotFound("管理员未找到");
    }

    private static async Task<IResult> AdminRegister(
        [FromServices] IEasyHasher<DefaultSHA256Digest> hasher,
        [FromServices] ServerDbContext dbContext,
        [FromBody] AdminRegisterRequest request)
    {
        var admin = new Admin
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            Password = hasher.GetHashedString(request.Password),
            Email = request.Email,
            Enable = true
        };
        await dbContext.Admins.AddAsync(admin);
        await dbContext.SaveChangesAsync();
        return Results.Created($"/admin/{admin.Id}", admin);
    }

    private async Task<IResult> AdminLogin(
        [FromServices] ServerDbContext dbContext,
        [FromServices] IEasyHasher<DefaultSHA256Digest> hasher,
        [FromBody] AdminLoginRequest adminLoginRequest,
        HttpContext context)
    {
        if (await dbContext.Admins.FirstOrDefaultAsync(a =>
                a.UserName == adminLoginRequest.UserName || a.Email == adminLoginRequest.UserName) is not { } admin)
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

public class AdminRegisterValidation : AbstractValidator<AdminRegisterRequest>
{
    public AdminRegisterValidation()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
    }
}
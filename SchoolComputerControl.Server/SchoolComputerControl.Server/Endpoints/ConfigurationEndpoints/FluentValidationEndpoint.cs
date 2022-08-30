using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using SchoolComputerControl.Server.Interfaces;

namespace SchoolComputerControl.Server.Endpoints.ConfigurationEndpoints;

public class FluentValidationEndpoint : IEndpoint
{
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblyContaining<FluentValidationEndpoint>(ServiceLifetime.Singleton);
    }

    public void ConfigureApp(WebApplication app)
    {
        // Nothing
    }
}

public class ValidationFilter<T> : IRouteHandlerFilter where T : class
{
    private readonly IValidator<T> _validator;

    public ValidationFilter(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(RouteHandlerInvocationContext context, RouteHandlerFilterDelegate next)
    {
        if (context.Arguments.FirstOrDefault(t => t?.GetType() == typeof(T)) is not T validatableObject)
            return BetterResults.Error("错误的请求数据");
        var validationResult = await _validator.ValidateAsync(validatableObject);
        if (!validationResult.IsValid)
        {
            return BetterResults.Error(string.Join(';', validationResult.Errors.Select(t => t.ErrorMessage)));
        }

        return await next(context);
    }
}
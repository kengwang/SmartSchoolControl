// ReSharper disable once CheckNamespace

namespace Microsoft.AspNetCore.Http;

public static partial class BetterResults
{
    public static IResult NotFound(string message = "找不到资源") => Error(message, 404);

    public static IResult Error(string message, int statusCode = 400)
    {
        return Problem(message, statusCode: statusCode);
    }

    private static IResult Problem(
        string? detail = null,
        string? instance = null,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        IDictionary<string, object?>? extensions = null)
        => TypedResults.Problem(detail, instance, statusCode, title, type, extensions);

    public static IResult Ok(object? value = null)
        => Ok<object>(value);

    public static IResult Ok<TValue>(TValue? value)
        => value is null ? TypedResults.Ok() : TypedResults.Ok(value);
}
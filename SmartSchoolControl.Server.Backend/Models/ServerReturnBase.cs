using Microsoft.AspNetCore.Mvc;
using SmartSchoolControl.Common.Base.ApiModels.Packages.Returns;

namespace SmartSchoolControl.Server.Backend.Models;

public class ServerReturnBase<TPackage> : ObjectResult where TPackage : class
{
    public ServerReturnBase(ServerReturnModel<TPackage> model, int httpStatusCode = 200) : base(model)
    {
        StatusCode = httpStatusCode;
    }

    public ServerReturnBase(TPackage? data = null) : base(new ServerReturnModel<TPackage>(true, "success", 200, data))
    {
        StatusCode = 200;
    }

    public ServerReturnBase(bool status = true, string message = "success", int code = 200,
        TPackage? data = null,
        int httpStatusCode = 200) : base(new ServerReturnModel<TPackage>(status, message, code, data))
    {
        StatusCode = httpStatusCode;
    }

    public ServerReturnBase(bool status = true, string message = "success", int code = 200,
        int httpStatusCode = 200) : base(new ServerReturnModel<TPackage>(status, message, code))
    {
        StatusCode = httpStatusCode;
    }
    
    public static readonly ServerReturnBase<TPackage> NotFound = new(new ServerReturnModel<TPackage>(false, "Not Found", -404), 404);
    public static readonly ServerReturnBase<TPackage> Ok = new(new ServerReturnModel<TPackage>());
    public static ServerReturnBase<TPackage> BadRequest = new(new ServerReturnModel<TPackage>(false, "Bad Request", -400), 400);

    public static ServerReturnBase<TPackage> ParamNotCompleted =
        new(new ServerReturnModel<TPackage>(false, "Incomplete parameters", -400), 400);
}

public class ServerReturnBase : ObjectResult
{
    public static readonly ServerReturnBase NotFound = new(new ServerReturnModel(false, "Not Found", -404), 404);
    public static readonly ServerReturnBase Ok = new(new ServerReturnModel());
    public static ServerReturnBase BadRequest = new(new ServerReturnModel(false, "Bad Request", -400), 400);

    public static ServerReturnBase ParamNotCompleted =
        new(new ServerReturnModel(false, "Incomplete parameters", -400), 400);

    public ServerReturnBase(ServerReturnModel model, int httpStatusCode = 200) : base(model)
    {
        StatusCode = httpStatusCode;
    }

    public ServerReturnBase(bool status = true, string message = "success", int code = 200,
        int httpStatusCode = 200) : base(new ServerReturnModel(status, message, code))
    {
        StatusCode = httpStatusCode;
    }
}
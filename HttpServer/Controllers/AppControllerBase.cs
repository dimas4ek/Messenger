using Application.Utils;
using Contracts.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HttpServer.Controllers;

[ApiController]
public abstract class AppControllerBase : ControllerBase
{
    protected bool TryGetValue<T>(Result<T> result, out T value, out ObjectResult error)
    {
        if (!result.IsSuccess)
        {
            value = default!;
            error = BadRequest(new ErrorResponse { ErrorCode = result.ErrorCode });
            return false;
        }

        value = result.Value;
        error = null!;
        return true;
    }

    protected bool TryGetValue(Result result, out ObjectResult error)
    {
        if (!result.IsSuccess)
        {
            error = BadRequest(new ErrorResponse { ErrorCode = result.ErrorCode });
            return false;
        }

        error = null!;
        return true;
    }
}
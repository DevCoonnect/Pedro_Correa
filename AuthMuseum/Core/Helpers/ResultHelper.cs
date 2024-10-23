using AuthMuseum.Core.Common;
using AuthMuseum.Domain.Errors;
using Microsoft.AspNetCore.Mvc;

namespace AuthMuseum.Core.Helpers;
public static class ResultHelper
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.Match(
            (value) => new OkObjectResult(value),
            ErrorToActionResult
        );
    }

    public static IActionResult ErrorToActionResult(this Error error)
    {
        return error switch
        {
            UnprocessableEntityError => new UnprocessableEntityObjectResult(error.Message),
            NotFoundError => new NotFoundObjectResult(error.Message),
            BadRequestError => new BadRequestObjectResult(error.Message),
            ConflictError => new ConflictObjectResult(error.Message),
            UnauthorizedError => new UnauthorizedObjectResult(error.Message),
            ForbiddenError => new ObjectResult(error.Message)
            {
                StatusCode = 403
            },
            InternalServerError => new ObjectResult(error.Message)
            {
                StatusCode = 500
            },
            _ => new ObjectResult(InternalServerError.DefaultInternalServerErrorMessage)
            {
                StatusCode = 500
            },
        };
    }
}

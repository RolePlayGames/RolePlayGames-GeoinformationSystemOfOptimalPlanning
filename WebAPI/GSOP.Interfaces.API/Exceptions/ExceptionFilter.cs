using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GSOP.Interfaces.API.Exceptions;

public abstract class ExceptionFilterBase : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var mapResult = MapException(context.Exception);

        context.Result = mapResult is not null
            ? mapResult
            : context.Exception switch
            {
                ArgumentException argumentException => new BadRequestObjectResult(argumentException.Message),
                NotImplementedException => new StatusCodeResult(StatusCodes.Status501NotImplemented),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError),
            };
    }

    protected abstract IActionResult? MapException(Exception exception);
}

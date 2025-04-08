using GSOP.Domain.Contracts.Routes.Exceptions;
using GSOP.Interfaces.API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GSOP.Interfaces.API.Routes;

public class RoutesExceptionFilter : ExceptionFilterBase
{
    protected override IActionResult? MapException(Exception exception) => exception switch
    {
        RouteWasNotFoundException => new StatusCodeResult(StatusCodes.Status404NotFound),
        _ => null,
    };
}

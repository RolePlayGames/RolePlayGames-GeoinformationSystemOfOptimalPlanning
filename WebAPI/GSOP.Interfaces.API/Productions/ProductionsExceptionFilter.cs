using GSOP.Domain.Contracts.Productions.Exceptions;
using GSOP.Interfaces.API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GSOP.Interfaces.API.Productions;

public class ProductionsExceptionFilter : ExceptionFilterBase
{
    protected override IActionResult? MapException(Exception exception) => exception switch
    {
        ProductionWasNotFoundException => new StatusCodeResult(StatusCodes.Status404NotFound),
        ProductionNameAlreadyExistsException => new UnprocessableEntityObjectResult(nameof(ProductionNameAlreadyExistsException)),
        _ => null,
    };
}

using GSOP.Domain.Contracts.ProductionLines.Exceptions;
using GSOP.Interfaces.API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GSOP.Interfaces.API.ProductionLines;

public class ProductionLinesExceptionFilter : ExceptionFilterBase
{
    protected override IActionResult? MapException(Exception exception) => exception switch
    {
        ProductionLineWasNotFoundException => new StatusCodeResult(StatusCodes.Status404NotFound),
        ProductionLineNameAlreadyExistsException => new UnprocessableEntityObjectResult(nameof(ProductionLineNameAlreadyExistsException)),
        _ => null,
    };
}

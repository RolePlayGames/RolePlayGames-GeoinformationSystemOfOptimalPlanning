using GSOP.Domain.Contracts.Orders.Exceptions;
using GSOP.Interfaces.API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GSOP.Interfaces.API.Orders;

public class OrdersExceptionFilter : ExceptionFilterBase
{
    protected override IActionResult? MapException(Exception exception) => exception switch
    {
        OrderWasNotFoundException => new StatusCodeResult(StatusCodes.Status404NotFound),
        OrderNumberAlreadyExistsException => new UnprocessableEntityObjectResult(nameof(OrderNumberAlreadyExistsException)),
        CustomerDoesNotExistsException => new UnprocessableEntityObjectResult(nameof(CustomerDoesNotExistsException)),
        FilmRecipeDoesNotExistsException => new UnprocessableEntityObjectResult(nameof(FilmRecipeDoesNotExistsException)),
        _ => null,
    };
}

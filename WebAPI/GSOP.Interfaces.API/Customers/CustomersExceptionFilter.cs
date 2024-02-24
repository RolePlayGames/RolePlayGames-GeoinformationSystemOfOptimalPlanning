using GSOP.Domain.Contracts.Customers.Exceptions;
using GSOP.Interfaces.API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GSOP.Interfaces.API.Customers;

public class CustomersExceptionFilter : ExceptionFilterBase
{
    protected override IActionResult? MapException(Exception exception) => exception switch
    {
        CustomerWasNotFoundException => new StatusCodeResult(StatusCodes.Status404NotFound),
        CustomerNameAlreadyExistsException => new UnprocessableEntityObjectResult(nameof(CustomerNameAlreadyExistsException)),
        _ => null,
    };
}

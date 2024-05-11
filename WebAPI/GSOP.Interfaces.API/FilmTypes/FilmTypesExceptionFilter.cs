using GSOP.Domain.Contracts.FilmTypes.Exceptions;
using GSOP.Interfaces.API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GSOP.Interfaces.API.FilmTypes;

public class FilmTypesExceptionFilter : ExceptionFilterBase
{
    protected override IActionResult? MapException(Exception exception) => exception switch
    {
        FilmTypeWasNotFoundException => new StatusCodeResult(StatusCodes.Status404NotFound),
        FilmTypeArticleAlreadyExistsException => new UnprocessableEntityObjectResult(nameof(FilmTypeArticleAlreadyExistsException)),
        _ => null,
    };
}

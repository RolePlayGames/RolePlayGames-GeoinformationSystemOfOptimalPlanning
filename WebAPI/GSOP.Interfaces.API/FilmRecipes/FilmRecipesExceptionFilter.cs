using GSOP.Domain.Contracts.FilmRecipes.Exceptions;
using GSOP.Interfaces.API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GSOP.Interfaces.API.FilmRecipes;

public class FilmRecipesExceptionFilter : ExceptionFilterBase
{
    protected override IActionResult? MapException(Exception exception) => exception switch
    {
        FilmRecipeWasNotFoundException => new StatusCodeResult(StatusCodes.Status404NotFound),
        FilmRecipeNameAlreadyExistsException => new UnprocessableEntityObjectResult(nameof(FilmRecipeNameAlreadyExistsException)),
        FilmTypeDoesNotExistsException => new UnprocessableEntityObjectResult(nameof(FilmTypeDoesNotExistsException)),
        _ => null,
    };
}

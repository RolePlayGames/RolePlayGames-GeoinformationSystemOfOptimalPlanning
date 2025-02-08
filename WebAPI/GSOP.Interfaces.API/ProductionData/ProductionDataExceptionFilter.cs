﻿using GSOP.Application.Contracts.ProductionData;
using GSOP.Domain.Contracts.Customers.Exceptions;
using GSOP.Domain.Contracts.FilmRecipes.Exceptions;
using GSOP.Domain.Contracts.FilmTypes.Exceptions;
using GSOP.Domain.Contracts.Orders.Exceptions;
using GSOP.Domain.Contracts.ProductionData;
using GSOP.Domain.Contracts.ProductionLines.Exceptions;
using GSOP.Interfaces.API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GSOP.Interfaces.API.ProductionData;

public class ProductionDataExceptionFilter : ExceptionFilterBase
{
    protected override IActionResult? MapException(Exception exception) => exception switch
    {
        CustomerNameAlreadyExistsException => new UnprocessableEntityObjectResult(nameof(CustomerNameAlreadyExistsException)),
        FilmRecipeNameAlreadyExistsException => new UnprocessableEntityObjectResult(nameof(FilmRecipeNameAlreadyExistsException)),
        FilmTypeDoesNotExistsException => new UnprocessableEntityObjectResult(nameof(FilmTypeDoesNotExistsException)),
        FilmTypeArticleAlreadyExistsException => new UnprocessableEntityObjectResult(nameof(FilmTypeArticleAlreadyExistsException)),
        OrderNumberAlreadyExistsException => new UnprocessableEntityObjectResult(nameof(OrderNumberAlreadyExistsException)),
        CustomerDoesNotExistsException => new UnprocessableEntityObjectResult(nameof(CustomerDoesNotExistsException)),
        FilmRecipeDoesNotExistsException => new UnprocessableEntityObjectResult(nameof(FilmRecipeDoesNotExistsException)),
        ProductionLineNameAlreadyExistsException => new UnprocessableEntityObjectResult(nameof(ProductionLineNameAlreadyExistsException)),
        ProductionDataEndImportException => new UnprocessableEntityObjectResult(nameof(ProductionDataEndImportException)),
        ProductionDataImportItemNotFoundException => new UnprocessableEntityObjectResult(nameof(ProductionDataImportItemNotFoundException)),
        _ => null,
    };
}

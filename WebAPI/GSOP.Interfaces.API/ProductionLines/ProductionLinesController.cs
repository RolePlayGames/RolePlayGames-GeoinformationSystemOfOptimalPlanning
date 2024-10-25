using GSOP.Application.Contracts.ProductionLines;
using GSOP.Domain.Contracts.ProductionLines.Models;
using Microsoft.AspNetCore.Mvc;

namespace GSOP.Interfaces.API.ProductionLines;

[ApiController]
[TypeFilter<ProductionLinesExceptionFilter>]
[Route("api/production-lines")]
public class ProductionLinesController
{
    private readonly ILogger<ProductionLinesController> _logger;
    private readonly IProductionLineService _productionLineService;

    public ProductionLinesController(ILogger<ProductionLinesController> logger, IProductionLineService productionLineService)
    {
        _logger = logger;
        _productionLineService = productionLineService;
    }

    [HttpPost]
    public Task<long> CreateFilmType(ProductionLineDTO productionLine)
    {
        return _productionLineService.CreateProductionLine(productionLine);
    }

    [HttpDelete]
    [Route("{id}")]
    public Task DeleteFilmType(long id)
    {
        return _productionLineService.DeleteProductionLine(id);
    }

    [HttpGet]
    [Route("{id}")]
    public Task<ProductionLineDTO> GetFilmType(long id)
    {
        return _productionLineService.GetProductionLine(id);
    }

    [HttpGet]
    [Route("info")]
    public Task<IReadOnlyCollection<ProductionLineInfo>> GetProductionLinesInfo()
    {
        return _productionLineService.GetProductionLinesInfo();
    }

    [HttpPost]
    [Route("{id}")]
    public Task UpdateCustomer(long id, ProductionLineDTO productionLine)
    {
        return _productionLineService.UpdateProductionLine(id, productionLine);
    }
}

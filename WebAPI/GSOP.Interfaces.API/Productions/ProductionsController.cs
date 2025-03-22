using GSOP.Application.Contracts.Productions;
using GSOP.Domain.Contracts.Productions.Models;
using Microsoft.AspNetCore.Mvc;

namespace GSOP.Interfaces.API.Productions;

[ApiController]
[TypeFilter<ProductionsExceptionFilter>]
[Route("api/productions")]
public class ProductionsController : ControllerBase
{
    private readonly ILogger<ProductionsController> _logger;
    private readonly IProductionService _productionService;

    public ProductionsController(ILogger<ProductionsController> logger, IProductionService productionService)
    {
        _logger = logger;
        _productionService = productionService;
    }

    [HttpPost]
    public Task<long> CreateProduction(ProductionDTO production)
    {
        return _productionService.CreateProduction(production);
    }

    [HttpDelete]
    [Route("{id}")]
    public Task DeleteProduction(long id)
    {
        return _productionService.DeleteProduction(id);
    }

    [HttpGet]
    [Route("{id}")]
    public Task<ProductionDTO> GetProduction(long id)
    {
        return _productionService.GetProduction(id);
    }

    [HttpGet]
    [Route("info")]
    public Task<IReadOnlyCollection<ProductionInfo>> GetProductionsInfo()
    {
        return _productionService.GetProductionsInfo();
    }

    [HttpPost]
    [Route("{id}")]
    public Task UpdateProduction(long id, ProductionDTO production)
    {
        return _productionService.UpdateProduction(id, production);
    }
}
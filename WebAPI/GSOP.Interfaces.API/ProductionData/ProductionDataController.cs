using GSOP.Application.Contracts.ProductionData;
using GSOP.Infrastructure.Excel.Contracts.ProductionData;
using Microsoft.AspNetCore.Mvc;

namespace GSOP.Interfaces.API.ProductionData;

[ApiController]
[TypeFilter<ProductionDataExceptionFilter>]
[Route("api/production-data")]
public class ProductionDataController : ControllerBase
{
    private readonly IProductionDataReader _productionDataReader;
    private readonly IProductionDataService _productionDataService;

    public ProductionDataController(IProductionDataReader productionDataReader, IProductionDataService productionDataService)
    {
        _productionDataReader = productionDataReader;
        _productionDataService = productionDataService;
    }

    [HttpPost]
    [Route("excel/import")]
    public Task Import(IFormFile file)
    {
        using var steam = file.OpenReadStream();

        var data = _productionDataReader.ReadProductionData(steam);

        return _productionDataService.Import(data);
    }
}

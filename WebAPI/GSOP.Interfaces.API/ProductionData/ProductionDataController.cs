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
    private readonly IProductionDataWriter _productionDataWriter;
    private readonly IProductionDataService _productionDataService;

    public ProductionDataController(IProductionDataReader productionDataReader, IProductionDataWriter productionDataWriter, IProductionDataService productionDataService)
    {
        _productionDataReader = productionDataReader;
        _productionDataWriter = productionDataWriter;
        _productionDataService = productionDataService;
    }

    [HttpGet]
    [Route("excel/export")]
    public async Task<IActionResult> Export()
    {
        var productionnData = await _productionDataService.Export();
        var excelStream = await _productionDataWriter.Write(productionnData);

        return File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProductionData.xlsx");
    }

    [HttpPost]
    [Route("excel/import")]
    public async Task<ProductionDataImportResult> Import(IFormFile file)
    {
        using var steam = file.OpenReadStream();

        var data = _productionDataReader.ReadProductionData(steam);

        await _productionDataService.Import(data, true);

        return new ProductionDataImportResult(data.Orders.Count, data.ProductionLines.Count);
    }
}
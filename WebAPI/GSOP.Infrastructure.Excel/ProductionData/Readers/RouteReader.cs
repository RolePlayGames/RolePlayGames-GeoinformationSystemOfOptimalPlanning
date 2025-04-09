using GSOP.Application.Contracts.ProductionData.Models;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Readers;

public class RouteReader : ModelReader<RouteModel>
{
    protected override int WorkSheetNum => 10;

    protected override RouteModel? ReadModel(ExcelRange cells, int rowNum)
    {
        var productionName = cells[rowNum, 1].Value?.ToString();
        var customerName = cells[rowNum, 2].Value?.ToString();
        var price = cells[rowNum, 3].Value?.ToString();
        var drivingTimeMinutes = cells[rowNum, 4].Value?.ToString();

        return productionName is null
            || productionName == string.Empty
            || customerName is null
            || customerName == string.Empty
            || !double.TryParse(price, out var priceNum)
            || !int.TryParse(drivingTimeMinutes, out var drivingTimeMinutesNum)
            ? null
            : new()
            {
                ProductionName = productionName,
                CustomerName = customerName,
                Price = priceNum,
                DrivingTimeMinutes = drivingTimeMinutesNum,
            };
    }
}

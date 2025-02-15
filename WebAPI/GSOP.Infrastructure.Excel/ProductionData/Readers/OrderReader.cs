using GSOP.Application.Contracts.ProductionData.Models;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Readers;

public class OrderReader : ModelReader<OrderModel>
{
    protected override int WorkSheetNum => 3;

    protected override OrderModel? ReadModel(ExcelRange cells, int rowNum)
    {
        var number = cells[rowNum, 1].Value?.ToString();
        var customerName = cells[rowNum, 2].Value?.ToString();
        var filmRecipeName = cells[rowNum, 3].Value?.ToString();
        var width = cells[rowNum, 4].Value?.ToString();
        var quantityInRunningMeter = cells[rowNum, 5].Value?.ToString();
        var finishedGoods = cells[rowNum, 6].Value?.ToString();
        var waste = cells[rowNum, 7].Value?.ToString();
        var rollsCount = cells[rowNum, 8].Value?.ToString();
        var plannedDate = cells[rowNum, 9].GetValue<DateTime?>();
        var priceOverdue = cells[rowNum, 10].Value?.ToString();

        return number is null
            || number == string.Empty
            || !int.TryParse(width, out var widthNum)
            || !int.TryParse(quantityInRunningMeter, out var quantityInRunningMeterNum)
            || !int.TryParse(finishedGoods, out var finishedGoodsNum)
            || !int.TryParse(waste, out var wasteNum)
            || !int.TryParse(rollsCount, out var rollsCountNum)
            || filmRecipeName is null
            || filmRecipeName == string.Empty
            || !double.TryParse(priceOverdue, out var priceOverdueNum)
            || customerName is null
            || customerName == string.Empty
            ? null
            : new()
            {
                Number = number,
                Width = widthNum,
                QuantityInRunningMeter = quantityInRunningMeterNum,
                FinishedGoods = finishedGoodsNum,
                Waste = wasteNum,
                RollsCount = rollsCountNum,
                FilmRecipeName = filmRecipeName,
                PlannedDate = plannedDate,
                PriceOverdue = priceOverdueNum,
                CustomerName = customerName,
            };
    }
}

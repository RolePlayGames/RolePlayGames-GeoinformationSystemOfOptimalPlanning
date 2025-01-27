using GSOP.Application.Contracts.ProductionData.Models;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Models;

public class ProductionLineReader : ModelReader<ProductionLineModel>
{
    protected override int WorkSheetNum => 4;

    protected override ProductionLineModel? ReadModel(ExcelRange cells, int rowNum)
    {
        var name = cells[rowNum, 1].Value?.ToString();
        var hourCost = cells[rowNum, 3].Value?.ToString();
        var maxProductionSpeed = cells[rowNum, 4].Value?.ToString();
        var widthMin = cells[rowNum, 5].Value?.ToString();
        var widthMax = cells[rowNum, 6].Value?.ToString();
        var thicknessMin = cells[rowNum, 7].Value?.ToString();
        var thicknessMax = cells[rowNum, 8].Value?.ToString();
        var thicknessChangeTime = cells[rowNum, 13].Value?.ToString();
        var thicknessChangeConsumption = cells[rowNum, 14].Value?.ToString();
        var widthChangeTime = cells[rowNum, 15].Value?.ToString();
        var widthChangeConsumption = cells[rowNum, 16].Value?.ToString();
        var setupTime = cells[rowNum, 17].Value?.ToString();

        return name is null
            || name == string.Empty
            || !decimal.TryParse(hourCost, out var hourCostNum)
            || !double.TryParse(maxProductionSpeed, out var maxProductionSpeedNum)
            || !double.TryParse(widthMin, out var widthMinNum)
            || !double.TryParse(widthMax, out var widthMaxNum)
            || !double.TryParse(thicknessMin, out var thicknessMinNum)
            || !double.TryParse(thicknessMax, out var thicknessMaxNum)
            || !TimeSpan.TryParse(thicknessChangeTime, out var thicknessChangeTimeSpan)
            || !double.TryParse(thicknessChangeConsumption, out var thicknessChangeConsumptionNum)
            || !TimeSpan.TryParse(widthChangeTime, out var widthChangeTimeSpan)
            || !double.TryParse(widthChangeConsumption, out var widthChangeConsumptionNum)
            || !TimeSpan.TryParse(setupTime, out var setupTimeSpan)
            ? null
            : new()
            {
                Name = name,
                HourCost = hourCostNum,
                MaxProductionSpeed = maxProductionSpeedNum,
                WidthMin = widthMinNum,
                WidthMax = widthMaxNum,
                ThicknessMin = thicknessMinNum,
                ThicknessMax = thicknessMaxNum,
                ThicknessChangeTime = thicknessChangeTimeSpan,
                ThicknessChangeConsumption = thicknessChangeConsumptionNum,
                WidthChangeTime = widthChangeTimeSpan,
                WidthChangeConsumption = widthChangeConsumptionNum,
                SetupTime = setupTimeSpan,
            };
    }
}

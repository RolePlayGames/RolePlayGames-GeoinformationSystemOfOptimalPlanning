using GSOP.Application.Contracts.ProductionData.Models.ChangeRules;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Readers;

public class CalibratoinChangeRuleReader : ModelReader<CalibratoinChangeRuleModel>
{
    protected override int WorkSheetNum => 5;

    protected override CalibratoinChangeRuleModel? ReadModel(ExcelRange cells, int rowNum)
    {
        var productionLineName = cells[rowNum, 1].Value?.ToString();
        var calibrationTo = cells[rowNum, 2].Value?.ToString();
        var changeTimeMinutes = cells[rowNum, 3].Value?.ToString();
        var changeConsumption = cells[rowNum, 4].Value?.ToString();

        return productionLineName is null
            || productionLineName == string.Empty
            || !double.TryParse(calibrationTo, out var calibrationToNum)
            || !int.TryParse(changeTimeMinutes, out var changeTimeMinutesNum)
            || !double.TryParse(changeConsumption, out var changeConsumptionNum)
            ? null
            : new()
            {
                ProductionLineName = productionLineName,
                CalibrationTo = calibrationToNum,
                ChangeTimeMinutes = changeTimeMinutesNum,
                ChangeConsumption = changeConsumptionNum,
            };
    }
}

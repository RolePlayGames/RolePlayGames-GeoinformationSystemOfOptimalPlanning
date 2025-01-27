using GSOP.Application.Contracts.ProductionData.Models.ChangeRules;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Models;

public class NozzleChangeRuleReader : ModelReader<NozzleChangeRuleModel>
{
    protected override int WorkSheetNum => 6;

    protected override NozzleChangeRuleModel? ReadModel(ExcelRange cells, int rowNum)
    {
        var productionLineName = cells[rowNum, 1].Value?.ToString();
        var nozzleTo = cells[rowNum, 2].Value?.ToString();
        var changeTimeMinutes = cells[rowNum, 3].Value?.ToString();
        var changeConsumption = cells[rowNum, 4].Value?.ToString();

        return productionLineName is null
            || productionLineName == string.Empty
            || !double.TryParse(nozzleTo, out var nozzleToNum)
            || !int.TryParse(changeTimeMinutes, out var changeTimeMinutesNum)
            || !double.TryParse(changeConsumption, out var changeConsumptionNum)
            ? null
            : new()
            {
                ProductionLineName = productionLineName,
                NozzleTo = nozzleToNum,
                ChangeTimeMinutes = changeTimeMinutesNum,
                ChangeConsumption = changeConsumptionNum,
            };
    }
}

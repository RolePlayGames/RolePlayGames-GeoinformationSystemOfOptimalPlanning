using GSOP.Application.Contracts.ProductionData.Models.ChangeRules;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Readers;

public class CoolingLipChangeRuleReader : ModelReader<CoolingLipChangeRuleModel>
{
    protected override int WorkSheetNum => 8;

    protected override CoolingLipChangeRuleModel? ReadModel(ExcelRange cells, int rowNum)
    {
        var productionLineName = cells[rowNum, 1].Value?.ToString();
        var coolingLipTo = cells[rowNum, 2].Value?.ToString();
        var changeTimeMinutes = cells[rowNum, 3].Value?.ToString();
        var changeConsumption = cells[rowNum, 4].Value?.ToString();

        return productionLineName is null
            || productionLineName == string.Empty
            || !double.TryParse(coolingLipTo, out var coolingLipToNum)
            || !int.TryParse(changeTimeMinutes, out var changeTimeMinutesNum)
            || !double.TryParse(changeConsumption, out var changeConsumptionNum)
            ? null
            : new()
            {
                ProductionLineName = productionLineName,
                CoolingLipTo = coolingLipToNum,
                ChangeTimeMinutes = changeTimeMinutesNum,
                ChangeConsumption = changeConsumptionNum,
            };
    }
}

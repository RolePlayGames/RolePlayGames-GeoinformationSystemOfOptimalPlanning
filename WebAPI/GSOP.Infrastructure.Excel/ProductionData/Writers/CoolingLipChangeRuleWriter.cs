using GSOP.Application.Contracts.ProductionData.Models.ChangeRules;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Writers;

public class CoolingLipChangeRuleWriter : ModelWriter<CoolingLipChangeRuleModel>
{
    protected override string WorsheetHeader => "CoolingLipChangeRules";

    protected override IReadOnlyCollection<string> Headers =>
    [
        nameof(CoolingLipChangeRuleModel.ProductionLineName),
        nameof(CoolingLipChangeRuleModel.CoolingLipTo),
        nameof(CoolingLipChangeRuleModel.ChangeTimeMinutes),
        nameof(CoolingLipChangeRuleModel.ChangeConsumption),
    ];

    protected override void WriteModel(ExcelWorksheet worksheet, CoolingLipChangeRuleModel model, int rowNum)
    {
        worksheet.Cells[rowNum, 1].Value = model.ProductionLineName;
        worksheet.Cells[rowNum, 2].Value = model.CoolingLipTo;
        worksheet.Cells[rowNum, 3].Value = model.ChangeTimeMinutes;
        worksheet.Cells[rowNum, 4].Value = model.ChangeConsumption;
    }
}

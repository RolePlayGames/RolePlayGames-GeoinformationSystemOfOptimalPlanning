using GSOP.Application.Contracts.ProductionData.Models.ChangeRules;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Writers;

public class CalibratoinChangeRuleWriter : ModelWriter<CalibratoinChangeRuleModel>
{
    protected override string WorsheetHeader => "CalibratoinChangeRules";

    protected override IReadOnlyCollection<string> Headers =>
    [
        nameof(CalibratoinChangeRuleModel.ProductionLineName),
        nameof(CalibratoinChangeRuleModel.CalibrationTo),
        nameof(CalibratoinChangeRuleModel.ChangeTimeMinutes),
        nameof(CalibratoinChangeRuleModel.ChangeConsumption),
    ];

    protected override void WriteModel(ExcelWorksheet worksheet, CalibratoinChangeRuleModel model, int rowNum)
    {
        worksheet.Cells[rowNum, 1].Value = model.ProductionLineName;
        worksheet.Cells[rowNum, 2].Value = model.CalibrationTo;
        worksheet.Cells[rowNum, 3].Value = model.ChangeTimeMinutes;
        worksheet.Cells[rowNum, 4].Value = model.ChangeConsumption;
    }
}

using GSOP.Application.Contracts.ProductionData.Models.ChangeRules;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Writers;

public class NozzleChangeRuleWriter : ModelWriter<NozzleChangeRuleModel>
{
    protected override string WorsheetHeader => "NozzleChangeRules";

    protected override IReadOnlyCollection<string> Headers =>
    [
        nameof(NozzleChangeRuleModel.ProductionLineName),
        nameof(NozzleChangeRuleModel.NozzleTo),
        nameof(NozzleChangeRuleModel.ChangeTimeMinutes),
        nameof(NozzleChangeRuleModel.ChangeConsumption),
    ];

    protected override void WriteModel(ExcelWorksheet worksheet, NozzleChangeRuleModel model, int rowNum)
    {
        worksheet.Cells[rowNum, 1].Value = model.ProductionLineName;
        worksheet.Cells[rowNum, 2].Value = model.NozzleTo;
        worksheet.Cells[rowNum, 3].Value = model.ChangeTimeMinutes;
        worksheet.Cells[rowNum, 4].Value = model.ChangeConsumption;
    }
}

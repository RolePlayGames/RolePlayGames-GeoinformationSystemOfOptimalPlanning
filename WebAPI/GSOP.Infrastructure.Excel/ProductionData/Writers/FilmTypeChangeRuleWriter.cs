using GSOP.Application.Contracts.ProductionData.Models.ChangeRules;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Writers;

public class FilmTypeChangeRuleWriter : ModelWriter<FilmTypeChangeRuleModel>
{
    protected override string WorsheetHeader => "FilmTypeChangeRules";

    protected override IReadOnlyCollection<string> Headers =>
    [
        nameof(FilmTypeChangeRuleModel.ProductionLineName),
        nameof(FilmTypeChangeRuleModel.FilmRecipeFromArticle),
        nameof(FilmTypeChangeRuleModel.FilmRecipeFromArticle),
        nameof(FilmTypeChangeRuleModel.ChangeTimeMinutes),
        nameof(FilmTypeChangeRuleModel.ChangeConsumption),
    ];

    protected override void WriteModel(ExcelWorksheet worksheet, FilmTypeChangeRuleModel model, int rowNum)
    {
        worksheet.Cells[rowNum, 1].Value = model.ProductionLineName;
        worksheet.Cells[rowNum, 2].Value = model.FilmRecipeFromArticle;
        worksheet.Cells[rowNum, 3].Value = model.FilmRecipeToArticle;
        worksheet.Cells[rowNum, 4].Value = model.ChangeTimeMinutes;
        worksheet.Cells[rowNum, 5].Value = model.ChangeConsumption;
    }
}

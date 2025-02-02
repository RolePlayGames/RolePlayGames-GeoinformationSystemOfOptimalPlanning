using GSOP.Application.Contracts.ProductionData.Models.ChangeRules;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Readers;

public class FilmTypeChangeRuleReader : ModelReader<FilmTypeChangeRuleModel>
{
    protected override int WorkSheetNum => 5;

    protected override FilmTypeChangeRuleModel? ReadModel(ExcelRange cells, int rowNum)
    {
        var productionLineName = cells[rowNum, 1].Value?.ToString();
        var filmRecipeFromArticle = cells[rowNum, 2].Value?.ToString();
        var filmRecipeToArticle = cells[rowNum, 3].Value?.ToString();
        var changeTimeMinutes = cells[rowNum, 4].Value?.ToString();
        var changeConsumption = cells[rowNum, 5].Value?.ToString();

        return productionLineName is null
            || productionLineName == string.Empty
            || filmRecipeFromArticle is null
            || filmRecipeFromArticle == string.Empty
            || filmRecipeToArticle is null
            || filmRecipeToArticle == string.Empty
            || !int.TryParse(changeTimeMinutes, out var changeTimeMinutesNum)
            || !double.TryParse(changeConsumption, out var changeConsumptionNum)
            ? null
            : new()
            {
                ProductionLineName = productionLineName,
                FilmRecipeFromArticle = filmRecipeFromArticle,
                FilmRecipeToArticle = filmRecipeToArticle,
                ChangeTimeMinutes = changeTimeMinutesNum,
                ChangeConsumption = changeConsumptionNum,
            };
    }
}

using GSOP.Application.Contracts.ProductionData.Models;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Readers;

public class FilmRecipeReader : ModelReader<FilmRecipeModel>
{
    protected override int WorkSheetNum => 1;

    protected override FilmRecipeModel? ReadModel(ExcelRange cells, int rowNum)
    {
        var name = cells[rowNum, 1].Value?.ToString();
        var filmTypeArticle = cells[rowNum, 2].Value?.ToString();
        var thickness = cells[rowNum, 3].Value?.ToString();
        var productionSpeed = cells[rowNum, 4].Value?.ToString();
        var materialCost = cells[rowNum, 5].Value?.ToString();
        var nozzle = cells[rowNum, 6].Value?.ToString();
        var calibration = cells[rowNum, 7].Value?.ToString();
        var coolingLip = cells[rowNum, 8].Value?.ToString();

        return name is null
            || name == string.Empty
            || filmTypeArticle is null
            || filmTypeArticle == string.Empty
            || !double.TryParse(thickness, out var thicknessNum)
            || !double.TryParse(productionSpeed, out var productionSpeedNum)
            || !double.TryParse(materialCost, out var materialCostNum)
            || !double.TryParse(nozzle, out var nozzleNum)
            || !double.TryParse(calibration, out var calibrationNum)
            || !double.TryParse(coolingLip, out var coolingLipNum)
            ? null
            : new()
            {
                Name = name,
                FilmTypeArticle = filmTypeArticle,
                Thickness = thicknessNum,
                ProductionSpeed = productionSpeedNum,
                MaterialCost = materialCostNum,
                Nozzle = nozzleNum,
                Calibration = calibrationNum,
                CoolingLip = coolingLipNum,
            };
    }
}

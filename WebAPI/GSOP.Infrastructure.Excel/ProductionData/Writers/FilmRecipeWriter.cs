using GSOP.Application.Contracts.ProductionData.Models;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Writers;

public class FilmRecipeWriter : ModelWriter<FilmRecipeModel>
{
    protected override string WorsheetHeader => "FilmRecipes";

    protected override IReadOnlyCollection<string> Headers =>
    [
        nameof(FilmRecipeModel.Name),
        nameof(FilmRecipeModel.FilmTypeArticle),
        nameof(FilmRecipeModel.Thickness),
        nameof(FilmRecipeModel.ProductionSpeed),
        nameof(FilmRecipeModel.MaterialCost),
        nameof(FilmRecipeModel.Nozzle),
        nameof(FilmRecipeModel.Calibration),
        nameof(FilmRecipeModel.CoolingLip),
    ];

    protected override void WriteModel(ExcelWorksheet worksheet, FilmRecipeModel model, int rowNum)
    {
        worksheet.Cells[rowNum, 1].Value = model.Name;
        worksheet.Cells[rowNum, 2].Value = model.FilmTypeArticle;
        worksheet.Cells[rowNum, 3].Value = model.Thickness;
        worksheet.Cells[rowNum, 4].Value = model.ProductionSpeed;
        worksheet.Cells[rowNum, 5].Value = model.MaterialCost;
        worksheet.Cells[rowNum, 6].Value = model.Nozzle;
        worksheet.Cells[rowNum, 7].Value = model.Calibration;
        worksheet.Cells[rowNum, 8].Value = model.CoolingLip;
    }
}

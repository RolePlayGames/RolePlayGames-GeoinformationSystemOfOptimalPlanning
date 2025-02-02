using GSOP.Application.Contracts.ProductionData.Models;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Writers;

public class FilmTypeWriter : ModelWriter<FilmTypeModel>
{
    protected override string WorsheetHeader => "FilmTypes";

    protected override IReadOnlyCollection<string> Headers => [nameof(FilmTypeModel.Article)];

    protected override void WriteModel(ExcelWorksheet worksheet, FilmTypeModel model, int rowNum)
    {
        worksheet.Cells[rowNum, 1].Value = model.Article;
    }
}

using GSOP.Application.Contracts.ProductionData.Models;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Readers;

public class FilmTypeReader : ModelReader<FilmTypeModel>
{
    protected override int WorkSheetNum => 0;

    protected override FilmTypeModel? ReadModel(ExcelRange cells, int rowNum)
    {
        var article = cells[rowNum, 1].Value?.ToString();

        return article is null || article == string.Empty ? null : new() { Article = article };
    }
}

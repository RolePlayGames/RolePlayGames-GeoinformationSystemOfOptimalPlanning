using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace GSOP.Infrastructure.Excel.ProductionData.Writers;

public abstract class ModelWriter<TModel> : IModelWriter<TModel>
{
    protected abstract string WorsheetHeader { get; }

    protected abstract IReadOnlyCollection<string> Headers { get; }

    public void Write(ExcelPackage package, IReadOnlyCollection<TModel> models)
    {
        var worksheet = package.Workbook.Worksheets.Add(WorsheetHeader);

        var headerCellNames = GenerateExcelCellNames(Headers.Count).ToList();

        for (var i = 0; i < headerCellNames.Count; i++)
        {
            worksheet.Cells[headerCellNames[i]].Value = Headers.ElementAt(i);
        }

        using (var range = worksheet.Cells[$"{headerCellNames.FirstOrDefault()}:${headerCellNames.LastOrDefault()}"])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        var rowNum = 2;

        foreach (var model in models)
        {
            WriteModel(worksheet, model, rowNum);
            rowNum++;
        }

        worksheet.Cells.AutoFitColumns();
    }

    protected abstract void WriteModel(ExcelWorksheet worksheet, TModel model, int rowNum);

    private static IEnumerable<string> GenerateExcelCellNames(int count)
    {
        for (var i = 0; i < count; i++)
        {
            yield return GetExcelColumnName(i);
        }
    }

    private static string GetExcelColumnName(int columnNumber)
    {
        var dividend = columnNumber;
        var columnName = string.Empty;

        while (dividend > 0)
        {
            var modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar(65 + modulo) + columnName;
            dividend = (dividend - modulo) / 26;
        }
        return columnName + "1"; // Adding "1" to make it A1, B1 etc.
    }
}

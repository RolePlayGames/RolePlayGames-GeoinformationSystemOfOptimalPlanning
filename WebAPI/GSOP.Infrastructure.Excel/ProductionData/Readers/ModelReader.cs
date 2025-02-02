using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Readers;

public abstract class ModelReader<TModel> : IModelReader<TModel>
{
    protected abstract int WorkSheetNum { get; }

    public IReadOnlyCollection<TModel> Read(ExcelPackage package)
    {
        var worksheet = package.Workbook.Worksheets[WorkSheetNum];
        var rowCount = worksheet.Dimension?.Rows ?? 0;
        var items = new List<TModel>(rowCount);

        for (var row = 2; row <= rowCount; row++) // excel starts from 1 + skipping header
        {
            var item = ReadModel(worksheet.Cells, row);

            if (item is not null)
                items.Add(item);
        }

        return items;
    }

    protected abstract TModel? ReadModel(ExcelRange cells, int rowNum);
}

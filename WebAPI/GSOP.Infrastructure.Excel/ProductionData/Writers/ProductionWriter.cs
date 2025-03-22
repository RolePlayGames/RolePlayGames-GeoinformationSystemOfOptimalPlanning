using GSOP.Application.Contracts.ProductionData.Models;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Writers;

public class ProductionWriter : ModelWriter<ProductionModel>
{
    protected override string WorsheetHeader => "Productions";

    protected override IReadOnlyCollection<string> Headers => [nameof(ProductionModel.Name)];

    protected override void WriteModel(ExcelWorksheet worksheet, ProductionModel model, int rowNum)
    {
        worksheet.Cells[rowNum, 1].Value = model.Name;
        worksheet.Cells[rowNum, 2].Value = model.Latitude;
        worksheet.Cells[rowNum, 3].Value = model.Longitude;
    }
}

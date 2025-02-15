using GSOP.Application.Contracts.ProductionData.Models;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Writers;

public class ProductionLineWriter : ModelWriter<ProductionLineModel>
{
    protected override string WorsheetHeader => "ProductionLines";

    protected override IReadOnlyCollection<string> Headers =>
    [
        nameof(ProductionLineModel.Name),
        nameof(ProductionLineModel.HourCost),
        nameof(ProductionLineModel.MaxProductionSpeed),
        nameof(ProductionLineModel.WidthMin),
        nameof(ProductionLineModel.WidthMax),
        nameof(ProductionLineModel.ThicknessMin),
        nameof(ProductionLineModel.ThicknessMax),
        nameof(ProductionLineModel.ThicknessChangeTimeMinutes),
        nameof(ProductionLineModel.ThicknessChangeConsumption),
        nameof(ProductionLineModel.WidthChangeTimeMinutes),
        nameof(ProductionLineModel.WidthChangeConsumption),
        nameof(ProductionLineModel.SetupTimeMinutes),
    ];

    protected override void WriteModel(ExcelWorksheet worksheet, ProductionLineModel model, int rowNum)
    {
        worksheet.Cells[rowNum, 1].Value = model.Name;
        worksheet.Cells[rowNum, 2].Value = model.HourCost;
        worksheet.Cells[rowNum, 3].Value = model.MaxProductionSpeed;
        worksheet.Cells[rowNum, 4].Value = model.WidthMin;
        worksheet.Cells[rowNum, 5].Value = model.WidthMax;
        worksheet.Cells[rowNum, 6].Value = model.ThicknessMin;
        worksheet.Cells[rowNum, 7].Value = model.ThicknessMax;
        worksheet.Cells[rowNum, 8].Value = model.ThicknessChangeTimeMinutes;
        worksheet.Cells[rowNum, 9].Value = model.ThicknessChangeConsumption;
        worksheet.Cells[rowNum, 10].Value = model.WidthChangeTimeMinutes;
        worksheet.Cells[rowNum, 11].Value = model.WidthChangeConsumption;
        worksheet.Cells[rowNum, 12].Value = model.SetupTimeMinutes;
    }
}

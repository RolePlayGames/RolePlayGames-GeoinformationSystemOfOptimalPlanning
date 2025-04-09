using GSOP.Application.Contracts.ProductionData.Models;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Writers;

public class RouteWriter : ModelWriter<RouteModel>
{
    protected override string WorsheetHeader => "Routes";

    protected override IReadOnlyCollection<string> Headers =>
    [
        nameof(RouteModel.ProductionName),
        nameof(RouteModel.CustomerName),
        nameof(RouteModel.Price),
        nameof(RouteModel.DrivingTimeMinutes),
    ];

    protected override void WriteModel(ExcelWorksheet worksheet, RouteModel model, int rowNum)
    {
        worksheet.Cells[rowNum, 1].Value = model.ProductionName;
        worksheet.Cells[rowNum, 2].Value = model.CustomerName;
        worksheet.Cells[rowNum, 3].Value = model.Price;
        worksheet.Cells[rowNum, 4].Value = model.DrivingTimeMinutes;
    }
}

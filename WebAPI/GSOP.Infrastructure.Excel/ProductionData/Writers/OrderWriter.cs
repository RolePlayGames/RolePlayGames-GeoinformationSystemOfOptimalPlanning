using GSOP.Application.Contracts.ProductionData.Models;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Writers;

public class OrderWriter : ModelWriter<OrderModel>
{
    protected override string WorsheetHeader => "Orders";

    protected override IReadOnlyCollection<string> Headers =>
    [
        nameof(OrderModel.Number),
        nameof(OrderModel.CustomerName),
        nameof(OrderModel.FilmRecipeName),
        nameof(OrderModel.Width),
        nameof(OrderModel.QuantityInRunningMeter),
        nameof(OrderModel.FinishedGoods),
        nameof(OrderModel.Waste),
        nameof(OrderModel.RollsCount),
        nameof(OrderModel.PlannedDate),
        nameof(OrderModel.PriceOverdue),
    ];

    protected override void WriteModel(ExcelWorksheet worksheet, OrderModel model, int rowNum)
    {
        worksheet.Cells[rowNum, 1].Value = model.Number;
        worksheet.Cells[rowNum, 2].Value = model.Width;
        worksheet.Cells[rowNum, 3].Value = model.QuantityInRunningMeter;
        worksheet.Cells[rowNum, 4].Value = model.FinishedGoods;
        worksheet.Cells[rowNum, 5].Value = model.Waste;
        worksheet.Cells[rowNum, 6].Value = model.RollsCount;
        worksheet.Cells[rowNum, 7].Value = model.FilmRecipeName;
        worksheet.Cells[rowNum, 8].Value = model.PlannedDate;
        worksheet.Cells[rowNum, 8].Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
        worksheet.Cells[rowNum, 9].Value = model.PriceOverdue;
        worksheet.Cells[rowNum, 10].Value = model.CustomerName;
    }
}

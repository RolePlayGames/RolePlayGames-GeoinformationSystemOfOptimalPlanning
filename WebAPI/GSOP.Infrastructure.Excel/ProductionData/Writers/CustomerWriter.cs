using GSOP.Application.Contracts.ProductionData.Models;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Writers;

public class CustomerWriter : ModelWriter<CustomerModel>
{
    protected override string WorsheetHeader => "Customers";

    protected override IReadOnlyCollection<string> Headers => [nameof(CustomerModel.Name)];

    protected override void WriteModel(ExcelWorksheet worksheet, CustomerModel model, int rowNum)
    {
        worksheet.Cells[rowNum, 1].Value = model.Name;
    }
}

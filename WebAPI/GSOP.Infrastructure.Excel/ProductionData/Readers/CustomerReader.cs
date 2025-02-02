using GSOP.Application.Contracts.ProductionData.Models;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Readers;

public class CustomerReader : ModelReader<CustomerModel>
{
    protected override int WorkSheetNum => 2;

    protected override CustomerModel? ReadModel(ExcelRange cells, int rowNum)
    {
        var name = cells[rowNum, 1].Value?.ToString();

        return name is null || name == string.Empty ? null : new() { Name = name };
    }
}

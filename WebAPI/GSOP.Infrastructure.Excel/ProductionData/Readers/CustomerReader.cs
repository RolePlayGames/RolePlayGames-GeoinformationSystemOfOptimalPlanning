using GSOP.Application.Contracts.ProductionData.Models;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Readers;

public class CustomerReader : ModelReader<CustomerModel>
{
    protected override int WorkSheetNum => 2;

    protected override CustomerModel? ReadModel(ExcelRange cells, int rowNum)
    {
        var name = cells[rowNum, 1].Value?.ToString();
        var latitude = cells[rowNum, 2].Value?.ToString();
        var longitude = cells[rowNum, 3].Value?.ToString();

        return !TryConvertToNullableDecimal(latitude, out var latitudeNum) || !TryConvertToNullableDecimal(longitude, out var longitudeNum)
            ? null
            : name is null || name == string.Empty ? null : new() { Name = name, Latitude = latitudeNum, Longitude = longitudeNum };
    }

    private static bool TryConvertToNullableDecimal(string? value, out decimal? @decimal)
    {
        if (decimal.TryParse(value, out var valueDec))
        {
            @decimal = valueDec;
            return true;
        }
        
        @decimal = null;

        return value is null || value == string.Empty;
    }
}

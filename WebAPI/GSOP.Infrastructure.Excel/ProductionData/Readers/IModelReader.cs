using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Readers;

/// <summary>
/// Manages models reading logic
/// </summary>
/// <typeparam name="TModel">Model type</typeparam>
public interface IModelReader<TModel>
{
    /// <summary>
    /// Reads models from excel sheet
    /// </summary>
    /// <param name="package">Excel package</param>
    /// <returns>Readed models</returns>
    IReadOnlyCollection<TModel> Read(ExcelPackage package);
}

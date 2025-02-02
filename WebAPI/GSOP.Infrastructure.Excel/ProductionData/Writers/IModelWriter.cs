using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Writers;

/// <summary>
/// Manages writing model to excel logic
/// </summary>
/// <typeparam name="TModel">Model type</typeparam>
public interface IModelWriter<TModel>
{
    /// <summary>
    /// Writes models to excel cells
    /// </summary>
    /// <param name="package">Excel file package</param>
    /// <param name="models">Models to write</param>
    void Write(ExcelPackage package, IReadOnlyCollection<TModel> models);
}

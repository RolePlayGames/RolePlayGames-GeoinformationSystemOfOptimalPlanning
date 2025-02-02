namespace GSOP.Infrastructure.Excel.Contracts.ProductionData;

/// <summary>
/// Production data excel reader
/// </summary>
public interface IProductionDataReader
{
    /// <summary>
    /// Reads production data from excel file
    /// </summary>
    /// <param name="fileStream">File</param>
    /// <returns>Production data</returns>
    Application.Contracts.ProductionData.ProductionData ReadProductionData(Stream fileStream);
}

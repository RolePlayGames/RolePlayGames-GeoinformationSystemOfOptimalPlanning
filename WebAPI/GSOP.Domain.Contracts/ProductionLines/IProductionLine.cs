using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.Contracts.ProductionLines;

public interface IProductionLine
{
    public ProductionLineName Name { get; init; }

    public ProductionLineHourCost HourCost { get; init; }

    public ProductionLineMaxProductionSpeed MaxProductionSpeed { get; init; }

    public ProductionLineWidthRange WidthRange { get; init; }

    public ProductionLineThicknessRange ThicknessRange { get; init; }

    public ProductionLineChangeThicknessRule ThicknessChangeRule { get; init; }

    public ProductionLineChangeWidthRule WidthChangeRule { get; init; }

    public ProductionLineSetupTime SetupTime { get; init; }
}

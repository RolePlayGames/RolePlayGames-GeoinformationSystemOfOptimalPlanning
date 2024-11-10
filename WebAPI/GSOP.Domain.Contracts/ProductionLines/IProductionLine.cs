using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.Contracts.ProductionLines;

public interface IProductionLine
{
    public ProductionLineName Name { get; }

    public ProductionLineHourCost HourCost { get; }

    public ProductionLineMaxProductionSpeed MaxProductionSpeed { get; }

    public ProductionLineWidthRange WidthRange { get; }

    public ProductionLineThicknessRange ThicknessRange { get; }

    public ProductionLineChangeThicknessRule ThicknessChangeRule { get; }

    public ProductionLineChangeWidthRule WidthChangeRule { get; }

    public ProductionLineSetupTime SetupTime { get; }

    Task SetName(ProductionLineName name);

    void SetHourCost(ProductionLineHourCost hourCost);

    void SetProductionLineMaxProductionSpeed(ProductionLineMaxProductionSpeed maxProductionSpeed);

    void SetProductionLineWidthRange(ProductionLineWidthRange widthRange);

    void SetProductionLineThicknessRange(ProductionLineThicknessRange thicknessRange);

    void SetProductionLineChangeThicknessRule(ProductionLineChangeThicknessRule thicknessChangeRule);

    void SetProductionLineChangeWidthRule(ProductionLineChangeWidthRule widthChangeRule);

    void SetProductionLineSetupTime(ProductionLineSetupTime setupTime);
}

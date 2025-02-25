using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.Contracts.ProductionLines;

public interface IProductionLine
{
    ProductionLineName Name { get; }

    ProductionLineHourCost HourCost { get; }

    ProductionLineMaxProductionSpeed MaxProductionSpeed { get; }

    ProductionLineWidthRange WidthRange { get; }

    ProductionLineThicknessRange ThicknessRange { get; }

    ProductionLineChangeThicknessRule ThicknessChangeRule { get; }

    ProductionLineChangeWidthRule WidthChangeRule { get; }

    ProductionLineSetupTime SetupTime { get; }

    IReadOnlyCollection<NozzleChangeRule> NozzleChangeRules { get; }

    IReadOnlyCollection<CalibratoinChangeRule> CalibratoinChangeRules { get; }

    IReadOnlyCollection<CoolingLipChangeRule> CoolingLipChangeRules { get; }

    IReadOnlyCollection<FilmTypeChangeRule> FilmTypeChangeRules { get; }

    Task SetName(ProductionLineName name);

    void SetHourCost(ProductionLineHourCost hourCost);

    void SetProductionLineMaxProductionSpeed(ProductionLineMaxProductionSpeed maxProductionSpeed);

    void SetProductionLineWidthRange(ProductionLineWidthRange widthRange);

    void SetProductionLineThicknessRange(ProductionLineThicknessRange thicknessRange);

    void SetProductionLineChangeThicknessRule(ProductionLineChangeThicknessRule thicknessChangeRule);

    void SetProductionLineChangeWidthRule(ProductionLineChangeWidthRule widthChangeRule);

    void SetProductionLineSetupTime(ProductionLineSetupTime setupTime);

    void SetNozzleChangeRules(IReadOnlyCollection<NozzleChangeRule> nozzleChangeRules);

    void SetCalibratoinChangeRules(IReadOnlyCollection<CalibratoinChangeRule> calibratoinChangeRules);

    void SetCoolingLipChangeRules(IReadOnlyCollection<CoolingLipChangeRule> coolingLipChangeRules);

    void SetFilmTypeChangeRules(IReadOnlyCollection<FilmTypeChangeRule> filmTypeChangeRules);
}

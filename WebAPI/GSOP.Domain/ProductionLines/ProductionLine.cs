using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Contracts.ProductionLines.Exceptions;
using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.ProductionLines;

public class ProductionLine : IProductionLine
{
    private readonly IProductionLineRepository _productionLineRepository;

    public ProductionLineName Name { get; private set; }

    public ProductionLineHourCost HourCost { get; private set; }

    public ProductionLineMaxProductionSpeed MaxProductionSpeed { get; private set; }

    public ProductionLineWidthRange WidthRange { get; private set; }

    public ProductionLineThicknessRange ThicknessRange { get; private set; }

    public ProductionLineChangeThicknessRule ThicknessChangeRule { get; private set; }

    public ProductionLineChangeWidthRule WidthChangeRule { get; private set; }

    public ProductionLineSetupTime SetupTime { get; private set; }

    public IReadOnlyCollection<ID> NozzleChangeRuleIDs { get; }

    public IReadOnlyCollection<ID> CalibratoinChangeRuleIDs { get; }

    public IReadOnlyCollection<ID> CoolingLipChangeRuleIDs { get; }

    public IReadOnlyCollection<ID> FilmTypeChangeRuleIDs { get; }

    public ProductionLine(
        ProductionLineName name,
        ProductionLineHourCost hourCost,
        ProductionLineMaxProductionSpeed maxProductionSpeed,
        ProductionLineWidthRange widthRange,
        ProductionLineThicknessRange thicknessRange,
        ProductionLineChangeThicknessRule thicknessChangeRule,
        ProductionLineChangeWidthRule widthChangeRule,
        ProductionLineSetupTime setupTime,
        IProductionLineRepository productionLineRepository)
    {
        Name = name;
        HourCost = hourCost;
        MaxProductionSpeed = maxProductionSpeed;
        WidthRange = widthRange;
        ThicknessRange = thicknessRange;
        ThicknessChangeRule = thicknessChangeRule;
        WidthChangeRule = widthChangeRule;
        SetupTime = setupTime;
        _productionLineRepository = productionLineRepository;
    }

    public void SetHourCost(ProductionLineHourCost hourCost)
    {
        HourCost = hourCost;
    }

    public async Task SetName(ProductionLineName name)
    {
        if (Name != name)
        {
            var isNumberExists = await _productionLineRepository.IsNameExists(name);

            if (isNumberExists)
                throw new ProductionLineNameAlreadyExistsException(name);

            Name = name;
        }
    }

    public void SetProductionLineChangeThicknessRule(ProductionLineChangeThicknessRule thicknessChangeRule)
    {
        ThicknessChangeRule = thicknessChangeRule;
    }

    public void SetProductionLineChangeWidthRule(ProductionLineChangeWidthRule widthChangeRule)
    {
        WidthChangeRule = widthChangeRule;
    }

    public void SetProductionLineMaxProductionSpeed(ProductionLineMaxProductionSpeed maxProductionSpeed)
    {
        MaxProductionSpeed = maxProductionSpeed;
    }

    public void SetProductionLineSetupTime(ProductionLineSetupTime setupTime)
    {
        SetupTime = setupTime;
    }

    public void SetProductionLineThicknessRange(ProductionLineThicknessRange thicknessRange)
    {
        ThicknessRange = thicknessRange;
    }

    public void SetProductionLineWidthRange(ProductionLineWidthRange widthRange)
    {
        WidthRange = widthRange;
    }
}

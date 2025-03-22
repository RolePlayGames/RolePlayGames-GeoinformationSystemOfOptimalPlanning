using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Contracts.ProductionLines.Exceptions;
using GSOP.Domain.Contracts.ProductionLines.Models;
using GSOP.Domain.Contracts.Productions;
using GSOP.Domain.Contracts.Productions.Exceptions;

namespace GSOP.Domain.ProductionLines;

public class ProductionLine : IProductionLine
{
    private readonly IProductionLineRepository _productionLineRepository;
    private readonly IProductionRepository _productionRepository;

    public ProductionLineName Name { get; private set; }

    public ProductionLineHourCost HourCost { get; private set; }

    public ProductionLineMaxProductionSpeed MaxProductionSpeed { get; private set; }

    public ProductionLineWidthRange WidthRange { get; private set; }

    public ProductionLineThicknessRange ThicknessRange { get; private set; }

    public ProductionLineChangeThicknessRule ThicknessChangeRule { get; private set; }

    public ProductionLineChangeWidthRule WidthChangeRule { get; private set; }

    public ProductionLineSetupTime SetupTime { get; private set; }

    public IReadOnlyCollection<NozzleChangeRule> NozzleChangeRules { get; private set; }

    public IReadOnlyCollection<CalibratoinChangeRule> CalibratoinChangeRules { get; private set; }

    public IReadOnlyCollection<CoolingLipChangeRule> CoolingLipChangeRules { get; private set; }

    public IReadOnlyCollection<FilmTypeChangeRule> FilmTypeChangeRules { get; private set; }

    public ID ProductionID { get; private set; }

    public ProductionLine(
        ProductionLineName name,
        ProductionLineHourCost hourCost,
        ProductionLineMaxProductionSpeed maxProductionSpeed,
        ProductionLineWidthRange widthRange,
        ProductionLineThicknessRange thicknessRange,
        ProductionLineChangeThicknessRule thicknessChangeRule,
        ProductionLineChangeWidthRule widthChangeRule,
        ProductionLineSetupTime setupTime,
        ID productionId,
        IReadOnlyCollection<NozzleChangeRule> nozzleChangeRules,
        IReadOnlyCollection<CalibratoinChangeRule> calibratoinChangeRsules,
        IReadOnlyCollection<CoolingLipChangeRule> coolingLipChangeRsules,
        IReadOnlyCollection<FilmTypeChangeRule> filmTypeChangeRsules,
        IProductionLineRepository productionLineRepository,
        IProductionRepository productionRepository)
    {
        Name = name;
        HourCost = hourCost;
        MaxProductionSpeed = maxProductionSpeed;
        WidthRange = widthRange;
        ThicknessRange = thicknessRange;
        ThicknessChangeRule = thicknessChangeRule;
        WidthChangeRule = widthChangeRule;
        SetupTime = setupTime;
        ProductionID = productionId;
        NozzleChangeRules = nozzleChangeRules;
        CalibratoinChangeRules = calibratoinChangeRsules;
        CoolingLipChangeRules = coolingLipChangeRsules;
        FilmTypeChangeRules = filmTypeChangeRsules;
        _productionLineRepository = productionLineRepository;
        _productionRepository = productionRepository;
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

    public void SetNozzleChangeRules(IReadOnlyCollection<NozzleChangeRule> nozzleChangeRules)
    {
        NozzleChangeRules = nozzleChangeRules;
    }

    public void SetCalibratoinChangeRules(IReadOnlyCollection<CalibratoinChangeRule> calibratoinChangeRules)
    {
        CalibratoinChangeRules = calibratoinChangeRules;
    }

    public void SetCoolingLipChangeRules(IReadOnlyCollection<CoolingLipChangeRule> coolingLipChangeRules)
    {
        CoolingLipChangeRules = coolingLipChangeRules;
    }

    public void SetFilmTypeChangeRules(IReadOnlyCollection<FilmTypeChangeRule> filmTypeChangeRules)
    {
        FilmTypeChangeRules = filmTypeChangeRules;
    }

    public async Task SetProductionID(ID productionId)
    {
        if (ProductionID != productionId)
        {
            var isProductionExists = await _productionRepository.IsProductionExists(productionId);

            if (!isProductionExists)
                throw new ProductionWasNotFoundException(productionId);

            ProductionID = productionId;
        }
    }
}

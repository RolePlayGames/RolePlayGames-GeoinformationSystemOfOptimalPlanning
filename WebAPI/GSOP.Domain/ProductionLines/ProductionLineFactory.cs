using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Contracts.ProductionLines.Exceptions;
using GSOP.Domain.Contracts.ProductionLines.Models;
using GSOP.Domain.Contracts.Productions;

namespace GSOP.Domain.ProductionLines;

public class ProductionLineFactory : IProductionLineFactory
{
    private readonly IProductionLineRepository _productionLineRepository;
    private readonly IProductionRepository _productionRepository;

    public ProductionLineFactory(IProductionLineRepository productionLineRepository, IProductionRepository productionRepository)
    {
        _productionLineRepository = productionLineRepository;
        _productionRepository = productionRepository;
    }

    public async Task<IProductionLine> Create(long id)
    {
        var productionLineID = new ID(id);

        var productionLine = await _productionLineRepository.Get(productionLineID) ?? throw new ProductionLineWasNotFoundException(productionLineID);

        var name = new ProductionLineName(productionLine.Name);
        var hourCost = new ProductionLineHourCost(productionLine.HourCost);
        var maxProductionSpeed = new ProductionLineMaxProductionSpeed(productionLine.MaxProductionSpeed);
        var widthRange = new ProductionLineWidthRange(productionLine.WidthMin, productionLine.WidthMax);
        var thicknessRange = new ProductionLineThicknessRange(productionLine.ThicknessMin, productionLine.ThicknessMax);
        var thicknessChangeRule = new ProductionLineChangeThicknessRule(productionLine.ThicknessChangeTime, productionLine.ThicknessChangeConsumption);
        var widthChangeRule = new ProductionLineChangeWidthRule(productionLine.WidthChangeTime, productionLine.WidthChangeConsumption);
        var setupTime = new ProductionLineSetupTime(productionLine.SetupTime);
        var nozzleChangeRules = productionLine.NozzleChangeRules.Select(x => new NozzleChangeRule { NozzleTo = new(x.NozzleTo), ChangeValueRule = new(x.ChangeTime, x.ChangeConsumption) }).ToList();
        var calibratoinChangeRules = productionLine.CalibratoinChangeRules.Select(x => new CalibratoinChangeRule { CalibrationTo = new(x.CalibrationTo), ChangeValueRule = new(x.ChangeTime, x.ChangeConsumption) }).ToList();
        var coolingLipChangeRules = productionLine.CoolingLipChangeRules.Select(x => new CoolingLipChangeRule { CoolingLipTo = new(x.CoolingLipTo), ChangeValueRule = new(x.ChangeTime, x.ChangeConsumption) }).ToList();
        var filmTypeChangeRules = productionLine.FilmTypeChangeRules.Select(x => new FilmTypeChangeRule { FilmTypeFromID = new(x.FilmRecipeFromID), FilmTypeToID = new(x.FilmRecipeToID), ChangeValueRule = new(x.ChangeTime, x.ChangeConsumption) }).ToList();
        var productionID = new ID(productionLine.ProductionID);

        return new ProductionLine(
            name,
            hourCost,
            maxProductionSpeed,
            widthRange,
            thicknessRange,
            thicknessChangeRule,
            widthChangeRule,
            setupTime,
            productionID,
            nozzleChangeRules,
            calibratoinChangeRules,
            coolingLipChangeRules,
            filmTypeChangeRules,
            _productionLineRepository,
            _productionRepository);
    }

    public async Task<IProductionLine> Create(ProductionLineDTO productionLine)
    {
        var name = new ProductionLineName(productionLine.Name);

        var isNameExsits = await _productionLineRepository.IsNameExists(name);

        if (isNameExsits)
            throw new ProductionLineNameAlreadyExistsException(name);

        var productionID = new ID(productionLine.ProductionID);
        var isProductionExists = await _productionRepository.IsProductionExists(productionID);

        var hourCost = new ProductionLineHourCost(productionLine.HourCost);
        var maxProductionSpeed = new ProductionLineMaxProductionSpeed(productionLine.MaxProductionSpeed);
        var widthRange = new ProductionLineWidthRange(productionLine.WidthMin, productionLine.WidthMax);
        var thicknessRange = new ProductionLineThicknessRange(productionLine.ThicknessMin, productionLine.ThicknessMax);
        var thicknessChangeRule = new ProductionLineChangeThicknessRule(productionLine.ThicknessChangeTime, productionLine.ThicknessChangeConsumption);
        var widthChangeRule = new ProductionLineChangeWidthRule(productionLine.WidthChangeTime, productionLine.WidthChangeConsumption);
        var setupTime = new ProductionLineSetupTime(productionLine.SetupTime);
        var nozzleChangeRules = productionLine.NozzleChangeRules.Select(x => new NozzleChangeRule { NozzleTo = new(x.NozzleTo), ChangeValueRule = new(x.ChangeTime, x.ChangeConsumption) }).ToList();
        var calibratoinChangeRules = productionLine.CalibratoinChangeRules.Select(x => new CalibratoinChangeRule { CalibrationTo = new(x.CalibrationTo), ChangeValueRule = new(x.ChangeTime, x.ChangeConsumption) }).ToList();
        var coolingLipChangeRules = productionLine.CoolingLipChangeRules.Select(x => new CoolingLipChangeRule { CoolingLipTo = new(x.CoolingLipTo), ChangeValueRule = new(x.ChangeTime, x.ChangeConsumption) }).ToList();
        var filmTypeChangeRules = productionLine.FilmTypeChangeRules.Select(x => new FilmTypeChangeRule { FilmTypeFromID = new(x.FilmRecipeFromID), FilmTypeToID = new(x.FilmRecipeToID), ChangeValueRule = new(x.ChangeTime, x.ChangeConsumption) }).ToList();

        return new ProductionLine(
            name,
            hourCost,
            maxProductionSpeed,
            widthRange,
            thicknessRange,
            thicknessChangeRule,
            widthChangeRule,
            setupTime,
            productionID,
            nozzleChangeRules,
            calibratoinChangeRules,
            coolingLipChangeRules,
            filmTypeChangeRules,
            _productionLineRepository,
            _productionRepository);
    }
}

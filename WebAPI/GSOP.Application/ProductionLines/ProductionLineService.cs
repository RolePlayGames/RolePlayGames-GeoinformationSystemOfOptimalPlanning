using GSOP.Application.Contracts.ProductionLines;
using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Contracts.ProductionLines.Exceptions;
using GSOP.Domain.Contracts.ProductionLines.Models;
using GSOP.Domain.Contracts.ProductionLines.ProductionRules;

namespace GSOP.Application.ProductionLines;

public class ProductionLineService : IProductionLineService
{
    private readonly IProductionLineFactory _productionLineFactory;
    private readonly IProductionLineRepository _productionLineRepository;

    public ProductionLineService(IProductionLineFactory productionLineFactory, IProductionLineRepository productionLineRepository)
    {
        _productionLineFactory = productionLineFactory;
        _productionLineRepository = productionLineRepository;
    }

    /// <inheritdoc/>
    public async Task<long> CreateProductionLine(ProductionLineDTO productionLine)
    {
        var newProductionLine = await _productionLineFactory.Create(productionLine);

        return await _productionLineRepository.Create(newProductionLine);
    }

    /// <inheritdoc/>
    public async Task DeleteProductionLine(long id)
    {
        var productionLineId = new ID(id);

        var isproductionLineDeleted = await _productionLineRepository.Delete(productionLineId);

        if (!isproductionLineDeleted)
            throw new ProductionLineWasNotFoundException(productionLineId);
    }

    /// <inheritdoc/>
    public async Task<ProductionLineDTO> GetProductionLine(long id)
    {
        var productionLineId = new ID(id);
        return await _productionLineRepository.Get(productionLineId) ?? throw new ProductionLineWasNotFoundException(productionLineId);
    }

    /// <inheritdoc/>
    public Task<IReadOnlyCollection<ProductionLineInfo>> GetProductionLinesInfo()
    {
        return _productionLineRepository.GetInfos();
    }

    /// <inheritdoc/>
    public async Task UpdateProductionLine(long id, ProductionLineDTO productionLine)
    {
        var productionLineID = new ID(id);
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

        var existingProductionLine = await _productionLineFactory.Create(id);

        await existingProductionLine.SetName(name);
        existingProductionLine.SetHourCost(hourCost);
        existingProductionLine.SetProductionLineMaxProductionSpeed(maxProductionSpeed);
        existingProductionLine.SetProductionLineWidthRange(widthRange);
        existingProductionLine.SetProductionLineThicknessRange(thicknessRange);
        existingProductionLine.SetProductionLineChangeThicknessRule(thicknessChangeRule);
        existingProductionLine.SetProductionLineChangeWidthRule(widthChangeRule);
        existingProductionLine.SetProductionLineSetupTime(setupTime);
        existingProductionLine.SetNozzleChangeRules(nozzleChangeRules);
        existingProductionLine.SetCalibratoinChangeRules(calibratoinChangeRules);
        existingProductionLine.SetCoolingLipChangeRules(coolingLipChangeRules);
        existingProductionLine.SetFilmTypeChangeRules(filmTypeChangeRules);

        await _productionLineRepository.Update(productionLineID, existingProductionLine);
    }
}

using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Contracts.ProductionLines.Models;
using GSOP.Domain.Contracts.ProductionLines.ProductionRules;
using GSOP.Infrastructure.DataAccess.ProductionLines.ProductionRules;
using LinqToDB;

namespace GSOP.Infrastructure.DataAccess.ProductionLines;

/// <inheritdoc/>
public class ProductionLineRepository : IProductionLineRepository
{
    private readonly DatabaseConnection _connection;

    public ProductionLineRepository(DatabaseConnection connection)
    {
        _connection = connection;
    }

    /// <inheritdoc/>
    public async Task<long> Create(IProductionLine productionLine)
    {
        using var transaction = await _connection.BeginTransactionAsync();

        var productionLineId = await _connection.InsertWithInt64IdentityAsync(new ProductionLinePOCO
        {
            Name = productionLine.Name,
            HourCost = productionLine.HourCost,
            MaxProductionSpeed = productionLine.MaxProductionSpeed,
            WidthMin = productionLine.WidthRange.MinWidth,
            WidthMax = productionLine.WidthRange.MaxWidth,
            ThicknessMin = productionLine.ThicknessRange.MinThickness,
            ThicknessMax = productionLine.ThicknessRange.MaxThickness,
            ThicknessChangeTime = productionLine.ThicknessChangeRule.ChangeTime,
            ThicknessChangeConsumption = productionLine.ThicknessChangeRule.ChangeConsumption,
            WidthChangeTime = productionLine.WidthChangeRule.ChangeTime,
            WidthChangeConsumption = productionLine.WidthChangeRule.ChangeConsumption,
            SetupTime = productionLine.SetupTime,
        });

        foreach (var cailbrationChangeRule in productionLine.CalibratoinChangeRules)
        {
            await _connection.InsertWithInt64IdentityAsync(new CalibrationChangeRulePOCO
            {
                ProductionLineID = productionLineId,
                CalibrationTo = cailbrationChangeRule.CalibrationTo,
                ChangeTime = cailbrationChangeRule.ChangeValueRule.ChangeTime,
                ChangeConsumption = cailbrationChangeRule.ChangeValueRule.ChangeConsumption,
            });
        }

        foreach (var coolingLipChangeRule in productionLine.CoolingLipChangeRules)
        {
            await _connection.InsertWithInt64IdentityAsync(new CoolingLipChangeRulePOCO
            {
                ProductionLineID = productionLineId,
                CoolingLipTo = coolingLipChangeRule.CoolingLipTo,
                ChangeTime = coolingLipChangeRule.ChangeValueRule.ChangeTime,
                ChangeConsumption = coolingLipChangeRule.ChangeValueRule.ChangeConsumption,
            });
        }

        foreach (var filmTypeChangeRule in productionLine.FilmTypeChangeRules)
        {
            await _connection.InsertWithInt64IdentityAsync(new FilmTypeChangeRulePOCO
            {
                ProductionLineID = productionLineId,
                FilmTypeFromID = filmTypeChangeRule.FilmRecipeFromID,
                FilmTypeToID = filmTypeChangeRule.FilmRecipeToID,
                ChangeTime = filmTypeChangeRule.ChangeValueRule.ChangeTime,
                ChangeConsumption = filmTypeChangeRule.ChangeValueRule.ChangeConsumption,
            });
        }

        foreach (var nozzleChangeRule in productionLine.NozzleChangeRules)
        {
            await _connection.InsertWithInt64IdentityAsync(new NozzleChangeRulePOCO
            {
                ProductionLineID = productionLineId,
                NozzleTo = nozzleChangeRule.NozzleTo,
                ChangeTime = nozzleChangeRule.ChangeValueRule.ChangeTime,
                ChangeConsumption = nozzleChangeRule.ChangeValueRule.ChangeConsumption,
            });
        }

        await transaction.CommitAsync();

        return productionLineId;
    }

    /// <inheritdoc/>
    public async Task<bool> Delete(ID id)
    {
        return await _connection.ProductionLines
            .Where(x => x.ID == id)
            .DeleteAsync() == 1;
    }

    /// <inheritdoc/>
    public async Task<ProductionLineDTO?> Get(ID id)
    {
        var calibratoinChangeRules = await _connection.CalibrationChangeRules
            .Where(x => x.ProductionLineID == id)
            .Select(x => new CalibratoinChangeRuleDTO { CalibrationTo = x.CalibrationTo, ChangeTime = x.ChangeTime, ChangeConsumption = x.ChangeConsumption })
            .ToListAsync();

        var coolingLipChangeRules = await _connection.CoolingLipChangeRules
            .Where(x => x.ProductionLineID == id)
            .Select(x => new CoolingLipChangeRuleDTO { CoolingLipTo = x.CoolingLipTo, ChangeConsumption = x.ChangeConsumption, ChangeTime = x.ChangeTime })
            .ToListAsync();

        var filmTypeChangeRules = await _connection.FilmTypeChangeRules
            .Where(x => x.ProductionLineID == id)
            .Select(x => new FilmTypeChangeRuleDTO { FilmRecipeFromID = x.FilmTypeFromID, FilmRecipeToID = x.FilmTypeToID, ChangeTime = x.ChangeTime, ChangeConsumption = x.ChangeConsumption })
            .ToListAsync();

        var nozzleChangeRules = await _connection.NozzleChangeRules
            .Where(x => x.ProductionLineID == id)
            .Select(x => new NozzleChangeRuleDTO { NozzleTo = x.NozzleTo, ChangeTime = x.ChangeTime, ChangeConsumption  = x.ChangeConsumption})
            .ToListAsync();

        var line = await _connection.ProductionLines
            .Where(x => x.ID == id)
            .Select(x => new ProductionLineDTO
            {
                Name = x.Name,
                HourCost = x.HourCost,
                MaxProductionSpeed = x.MaxProductionSpeed,
                WidthMin = x.WidthMin,
                WidthMax = x.WidthMax,
                ThicknessMin = x.ThicknessMin,
                ThicknessMax = x.ThicknessMax,
                ThicknessChangeTime = x.ThicknessChangeTime,
                ThicknessChangeConsumption = x.ThicknessChangeConsumption,
                WidthChangeTime = x.WidthChangeTime,
                WidthChangeConsumption = x.ThicknessChangeConsumption,
                SetupTime = x.SetupTime,
                CalibratoinChangeRules = calibratoinChangeRules,
                CoolingLipChangeRules = coolingLipChangeRules,
                FilmTypeChangeRules = filmTypeChangeRules,
                NozzleChangeRules = nozzleChangeRules,
            })
            .FirstOrDefaultAsync();

        return line;
    }
    /// <inheritdoc/>

    public async Task<IReadOnlyCollection<ProductionLineInfo>> GetInfos()
    {
        return await _connection.ProductionLines
            .Select(x => new ProductionLineInfo { ID = x.ID, Name = x.Name })
            .ToListAsync();
    }

    /// <inheritdoc/>
    public Task<bool> IsNameExists(ProductionLineName name)
    {
        return _connection.ProductionLines
            .Where(x => x.Name == name)
            .AnyAsync();
    }

    /// <inheritdoc/>
    public async Task Update(ID id, IProductionLine productionLine)
    {
        using var transaction = await _connection.BeginTransactionAsync();

        await _connection.CalibrationChangeRules.DeleteAsync(x => x.ProductionLineID == id);

        foreach (var cailbrationChangeRule in productionLine.CalibratoinChangeRules)
        {
            await _connection.InsertWithInt64IdentityAsync(new CalibrationChangeRulePOCO
            {
                ProductionLineID = id,
                CalibrationTo = cailbrationChangeRule.CalibrationTo,
                ChangeTime = cailbrationChangeRule.ChangeValueRule.ChangeTime,
                ChangeConsumption = cailbrationChangeRule.ChangeValueRule.ChangeConsumption,
            });
        }

        await _connection.CoolingLipChangeRules.DeleteAsync(x => x.ProductionLineID == id);

        foreach (var coolingLipChangeRule in productionLine.CoolingLipChangeRules)
        {
            await _connection.InsertWithInt64IdentityAsync(new CoolingLipChangeRulePOCO
            {
                ProductionLineID = id,
                CoolingLipTo = coolingLipChangeRule.CoolingLipTo,
                ChangeTime = coolingLipChangeRule.ChangeValueRule.ChangeTime,
                ChangeConsumption = coolingLipChangeRule.ChangeValueRule.ChangeConsumption,
            });
        }

        await _connection.FilmTypeChangeRules.DeleteAsync(x => x.ProductionLineID == id);

        foreach (var filmTypeChangeRule in productionLine.FilmTypeChangeRules)
        {
            await _connection.InsertWithInt64IdentityAsync(new FilmTypeChangeRulePOCO
            {
                ProductionLineID = id,
                FilmTypeFromID = filmTypeChangeRule.FilmRecipeFromID,
                FilmTypeToID = filmTypeChangeRule.FilmRecipeToID,
                ChangeTime = filmTypeChangeRule.ChangeValueRule.ChangeTime,
                ChangeConsumption = filmTypeChangeRule.ChangeValueRule.ChangeConsumption,
            });
        }

        await _connection.NozzleChangeRules.DeleteAsync(x => x.ProductionLineID == id);

        foreach (var nozzleChangeRule in productionLine.NozzleChangeRules)
        {
            await _connection.InsertWithInt64IdentityAsync(new NozzleChangeRulePOCO
            {
                ProductionLineID = id,
                NozzleTo = nozzleChangeRule.NozzleTo,
                ChangeTime = nozzleChangeRule.ChangeValueRule.ChangeTime,
                ChangeConsumption = nozzleChangeRule.ChangeValueRule.ChangeConsumption,
            });
        }

        await _connection.ProductionLines
            .Where(x => x.ID == id)
            .Set(x => x.Name, productionLine.Name)
            .Set(x => x.HourCost, productionLine.HourCost)
            .Set(x => x.MaxProductionSpeed, productionLine.MaxProductionSpeed)
            .Set(x => x.WidthMin, productionLine.WidthRange.MinWidth)
            .Set(x => x.WidthMax, productionLine.WidthRange.MaxWidth)
            .Set(x => x.ThicknessMin, productionLine.ThicknessRange.MinThickness)
            .Set(x => x.ThicknessMax, productionLine.ThicknessRange.MaxThickness)
            .Set(x => x.ThicknessChangeTime, productionLine.ThicknessChangeRule.ChangeTime)
            .Set(x => x.ThicknessChangeConsumption, productionLine.ThicknessChangeRule.ChangeConsumption)
            .Set(x => x.WidthChangeTime, productionLine.WidthChangeRule.ChangeTime)
            .Set(x => x.WidthChangeConsumption, productionLine.WidthChangeRule.ChangeConsumption)
            .Set(x => x.SetupTime, productionLine.SetupTime)
            .UpdateAsync();

        await transaction.CommitAsync();
    }
}

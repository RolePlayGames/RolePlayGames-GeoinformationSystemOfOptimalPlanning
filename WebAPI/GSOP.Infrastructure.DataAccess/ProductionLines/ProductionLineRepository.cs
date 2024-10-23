using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Contracts.ProductionLines.Models;
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
    public Task<long> Create(IProductionLine productionLine)
    {
        return _connection.InsertWithInt64IdentityAsync(new ProductionLinePOCO
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
    }

    /// <inheritdoc/>
    public async Task<bool> Delete(ID id)
    {
        return await _connection.ProductionLines
            .Where(x => x.ID == id)
            .DeleteAsync() == 1;
    }

    /// <inheritdoc/>
    public Task<ProductionLineDTO?> Get(ID id)
    {
        return _connection.ProductionLines
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
            })
            .FirstOrDefaultAsync();
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
    public Task Update(ID id, IProductionLine productionLine)
    {
        return _connection.ProductionLines
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
    }
}

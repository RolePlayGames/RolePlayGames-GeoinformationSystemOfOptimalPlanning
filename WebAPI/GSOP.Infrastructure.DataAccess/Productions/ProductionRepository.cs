using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.Locations;
using GSOP.Domain.Contracts.Productions;
using GSOP.Domain.Contracts.Productions.Models;
using LinqToDB;

namespace GSOP.Infrastructure.DataAccess.Productions;

/// <inheritdoc/>
public class ProductionRepository : IProductionRepository
{
    private readonly DatabaseConnection _connection;

    public ProductionRepository(DatabaseConnection connection)
    {
        _connection = connection;
    }

    /// <inheritdoc/>
    public Task<long> CreateProduction(IProduction production)
    {
        return _connection.InsertWithInt64IdentityAsync(new ProductionPOCO { Name = production.Name, Latitude = production.Coordinates?.Latitude, Longitude = production.Coordinates?.Longitude });
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteProduction(ID id)
    {
        return await _connection.Productions
            .Where(x => x.ID == id)
            .DeleteAsync() == 1;
    }

    /// <inheritdoc/>
    public Task<ProductionDTO?> GetProduction(ID id)
    {
        return _connection.Productions
            .Where(x => x.ID == id)
            .Select(x => new ProductionDTO { Name = x.Name, Coordinates = x.Latitude.HasValue && x.Longitude.HasValue ? new CoordinatesDTO { Latitude = x.Latitude.Value, Longitude = x.Longitude.Value } : null })
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<ProductionInfo>> GetProductionsInfo()
    {
        return await _connection.Productions
            .Select(x => new ProductionInfo { ID = x.ID, Name = x.Name })
            .ToListAsync();
    }

    /// <inheritdoc/>
    public Task UpdateProduction(ID id, IProduction production)
    {
        return _connection.Productions
            .Where(x => x.ID == id)
            .Set(x => x.Name, production.Name)
            .Set(x => x.Latitude, production.Coordinates?.Latitude)
            .Set(x => x.Longitude, production.Coordinates?.Longitude)
            .UpdateAsync();
    }

    /// <inheritdoc/>
    public Task<bool> IsProductionNameExsits(ProductionName productionName)
    {
        return _connection.Productions
            .Where(x => x.Name == productionName)
            .AnyAsync();
    }

    /// <inheritdoc/>
    public Task<bool> IsProductionExists(ID id)
    {
        return _connection.Productions
            .AnyAsync(x => x.ID == id);
    }
}

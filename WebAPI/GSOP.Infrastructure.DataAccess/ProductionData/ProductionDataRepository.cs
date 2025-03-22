using GSOP.Domain.Contracts.ProductionData;
using LinqToDB;

namespace GSOP.Infrastructure.DataAccess.ProductionData;

public class ProductionDataRepository : IProductionDataRepository
{
    private readonly DatabaseConnection _connection;

    public ProductionDataRepository(DatabaseConnection connection)
    {
        _connection = connection;
    }

    public async Task DeleteProductionData()
    {
        await _connection.CalibrationChangeRules.DeleteAsync();
        await _connection.CoolingLipChangeRules.DeleteAsync();
        await _connection.FilmTypeChangeRules.DeleteAsync();
        await _connection.NozzleChangeRules.DeleteAsync();
        await _connection.ProductionLines.DeleteAsync();
        await _connection.Orders.DeleteAsync();
        await _connection.Customers.DeleteAsync();
        await _connection.FilmRecipes.DeleteAsync();
        await _connection.FilmTypes.DeleteAsync();
        await _connection.Productions.DeleteAsync();
    }

    public async Task EndImport()
    {
        try
        {
            await _connection.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await _connection.RollbackTransactionAsync();
            throw new ProductionDataEndImportException(ex);
        }
    }

    public async Task<IAsyncDisposable> StartImport()
    {
        return await _connection.BeginTransactionAsync();
    }
}

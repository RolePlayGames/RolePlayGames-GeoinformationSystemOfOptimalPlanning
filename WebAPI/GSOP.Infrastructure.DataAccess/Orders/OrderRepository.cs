using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.Orders.Models;
using LinqToDB;

namespace GSOP.Infrastructure.DataAccess.Orders;

public class OrderRepository : IOrderRepository
{
    private readonly DatabaseConnection _connection;

    public OrderRepository(DatabaseConnection connection)
    {
        _connection = connection;
    }

    /// <inheritdoc/>
    public Task<long> Create(IOrder order)
    {
        return _connection.InsertWithInt64IdentityAsync(new OrderPOCO
        {
            Number = order.Number,
            CustomerID = order.CustomerID,
            FilmRecipeID = order.FilmRecipeID,
            Width = order.Width,
            QuantityInRunningMeter = order.QuantityInRunningMeter,
            FinishedGoods = order.FinishedGoods,
            Waste = order.Waste,
            RollsCount = order.RollsCount,
            PlannedDate = order.PlannedDate,
            PriceOverdue = order.PriceOverdue,
        });
    }

    /// <inheritdoc/>
    public async Task<bool> Delete(ID id)
    {
        return await _connection.Orders
            .Where(x => x.ID == id)
            .DeleteAsync() == 1;
    }

    /// <inheritdoc/>
    public Task<OrderDTO?> Get(ID id)
    {
        return _connection.Orders
            .Where(x => x.ID == id)
            .Select(x => new OrderDTO
            {
                Number = x.Number,
                CustomerID = x.CustomerID,
                FilmRecipeID = x.FilmRecipeID,
                Width = x.Width,
                QuantityInRunningMeter = x.QuantityInRunningMeter,
                FinishedGoods = x.FinishedGoods,
                Waste = x.Waste,
                RollsCount = x.RollsCount,
                PlannedDate = x.PlannedDate,
                PriceOverdue = x.PriceOverdue,
            })
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<OrderInfo>> GetInfos()
    {
        return await _connection.Orders
            .Select(x => new OrderInfo { ID = x.ID, Name = x.Number })
            .ToListAsync();
    }

    /// <inheritdoc/>
    public Task<bool> IsCustomerExists(CustomerID id)
    {
        return _connection.Customers.AnyAsync(x => x.ID == id);
    }

    /// <inheritdoc/>
    public Task<bool> IsFilmRecipeExists(FilmRecipeID id)
    {
        return _connection.FilmRecipes.AnyAsync(x => x.ID == id);
    }

    /// <inheritdoc/>
    public Task<bool> IsNumberExists(OrderNumber number)
    {
        return _connection.Orders.AnyAsync(x => x.Number == number);
    }

    /// <inheritdoc/>
    public Task Update(ID id, IOrder order)
    {
        return _connection.Orders
            .Where(x => x.ID == id)
            .Set(x => x.Number, order.Number)
            .Set(x => x.CustomerID, order.CustomerID)
            .Set(x => x.FilmRecipeID, order.FilmRecipeID)
            .Set(x => x.Width, order.Width)
            .Set(x => x.QuantityInRunningMeter, order.QuantityInRunningMeter)
            .Set(x => x.FinishedGoods, order.FinishedGoods)
            .Set(x => x.Waste, order.Waste)
            .Set(x => x.RollsCount, order.RollsCount)
            .Set(x => x.PlannedDate, order.PlannedDate)
            .Set(x => x.PriceOverdue, order.PriceOverdue)
            .UpdateAsync();
    }
}

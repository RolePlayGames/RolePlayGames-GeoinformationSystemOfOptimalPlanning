using GSOP.Domain.Contracts.Orders.Models;

namespace GSOP.Domain.Contracts.Orders;

/// <summary>
/// Manages order domain model creation
/// </summary>
public interface IOrderFactory
{
    /// <summary>
    /// Creates order by id from repository
    /// </summary>
    /// <param name="id">Order id</param>
    /// <returns>Order</returns>
    Task<IOrder> Create(long id);

    /// <summary>
    /// Creates and validates order by data
    /// </summary>
    /// <param name="order">Order data</param>
    /// <returns>Order</returns>
    Task<IOrder> Create(OrderDTO order);
}

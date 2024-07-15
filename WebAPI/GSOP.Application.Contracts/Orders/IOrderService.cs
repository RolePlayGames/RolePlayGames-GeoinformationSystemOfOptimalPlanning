using GSOP.Domain.Contracts.Orders.Models;

namespace GSOP.Application.Contracts.Orders;

/// <summary>
/// Manages order use cases
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Creates new order
    /// </summary>
    /// <param name="order">Order data</param>
    /// <returns>New order ID</returns>
    Task<long> CreateOrder(OrderDTO order);

    /// <summary>
    /// Deletes order
    /// </summary>
    /// <param name="id">Order ID</param>
    Task DeleteOrder(long id);

    /// <summary>
    /// Gets order by ID
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <returns>Order</returns>
    Task<OrderDTO> GetOrder(long id);

    /// <summary>
    /// Returns orders small information
    /// </summary>
    Task<IReadOnlyCollection<OrderInfo>> GetOrdersInfo();

    /// <summary>
    /// Updates order
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <param name="order">Order data</param>
    Task UpdateOrder(long id, OrderDTO order);
}

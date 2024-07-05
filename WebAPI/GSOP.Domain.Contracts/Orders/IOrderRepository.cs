using GSOP.Domain.Contracts.Orders.Models;

namespace GSOP.Domain.Contracts.Orders;

/// <summary>
/// Manages order database logic
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Creates order in database
    /// </summary>
    /// <param name="order">Order</param>
    /// <returns>Generated id</returns>
    Task<long> Create(IOrder order);

    /// <summary>
    /// Deletes order
    /// </summary>
    /// <param name="id">Order id</param>
    /// <returns>Is order deleted</returns>
    Task<bool> Delete(ID id);

    /// <summary>
    /// Gets order by id
    /// </summary>
    /// <param name="id">Order id</param>
    /// <returns>Order data or null</returns>
    Task<OrderDTO?> Get(ID id);

    /// <summary>
    /// Gets order short information
    /// </summary>
    /// <returns>Each order info</returns>
    Task<IReadOnlyCollection<OrderInfo>> GetInfos();

    /// <summary>
    /// Is customer already exists
    /// </summary>
    /// <param name="id">>Customer id</param>
    /// <returns>True if customer exists</returns>
    Task<bool> IsCustomerExists(CustomerID id);

    /// <summary>
    /// Is film recipe already exists
    /// </summary>
    /// <param name="id">>Film recipe id</param>
    /// <returns>True if film type exists</returns>
    Task<bool> IsFilmRecipeExists(FilmRecipeID id);

    /// <summary>
    /// Updates order
    /// </summary>
    /// <param name="id">Order id</param>
    /// <param name="order">Order</param>
    Task Update(ID id, IOrder order);
}

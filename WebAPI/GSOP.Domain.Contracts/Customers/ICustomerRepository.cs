using GSOP.Domain.Contracts.Customers.Models;

namespace GSOP.Domain.Contracts.Customers;

/// <summary>
/// Manages customer database logic
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// Creates customer in database
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <returns>Generated id</returns>
    Task<long> CreateCustomer(ICustomer customer);

    /// <summary>
    /// Deletes customer
    /// </summary>
    /// <param name="id">Customer id</param>
    /// <returns>Is customer deleted</returns>
    Task<bool> DeleteCustomer(ID id);

    /// <summary>
    /// Gets customer by id
    /// </summary>
    /// <param name="id">Customer id</param>
    /// <returns>Customer data or null</returns>
    Task<CustomerDTO?> GetCustomer(ID id);

    /// <summary>
    /// Gets customer short information
    /// </summary>
    /// <returns>Each customer info</returns>
    Task<IReadOnlyCollection<CustomerInfo>> GetCustomersInfo();

    /// <summary>
    /// Is customer name already exists
    /// </summary>
    /// <param name="customerName">Customer name</param>
    /// <returns>True if name is exists</returns>
    Task<bool> IsCustomerNameExsits(CustomerName customerName);

    /// <summary>
    /// Is customer already exists
    /// </summary>
    /// <param name="id">Customer id</param>
    /// <returns>True if customer exists</returns>
    Task<bool> IsCustomerExsits(ID id);

    /// <summary>
    /// Updates customer
    /// </summary>
    /// <param name="id">Customer id</param>
    /// <param name="customer">Customer</param>
    Task UpdateCustomer(ID id, ICustomer customer);
}

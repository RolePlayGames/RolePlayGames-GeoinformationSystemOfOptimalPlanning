using GSOP.Domain.Contracts.Customers.Models;

namespace GSOP.Domain.Contracts.Customers;

/// <summary>
/// Manages customer domain model creation
/// </summary>
public interface ICustomerFactory
{
    /// <summary>
    /// Creates customer by id from repository
    /// </summary>
    /// <param name="id">Customer id</param>
    /// <returns>Customer</returns>
    Task<ICustomer> CreateCustomer(long id);

    /// <summary>
    /// Creates and validates customer by customer data
    /// </summary>
    /// <param name="customer">Customer data</param>
    /// <returns>Customer</returns>
    Task<ICustomer> CreateCustomer(CustomerDTO customer);
}

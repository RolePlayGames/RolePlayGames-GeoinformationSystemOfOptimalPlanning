using GSOP.Domain.Contracts.Customers.Models;

namespace GSOP.Domain.Contracts.Customers;

/// <summary>
/// Customer domain model
/// </summary>
public interface ICustomer
{
    CustomerName Name { get; }

    /// <summary>
    /// Validates and updates customer name
    /// </summary>
    /// <param name="name">Customer name</param>
    Task SetName(CustomerName name);
}

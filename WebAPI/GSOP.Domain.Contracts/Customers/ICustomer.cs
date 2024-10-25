using GSOP.Domain.Contracts.Customers.Models;

namespace GSOP.Domain.Contracts.Customers;

/// <summary>
/// Customer domain model
/// </summary>
public interface ICustomer
{
    CustomerName Name { get; }

    Task SetName(CustomerName name);
}

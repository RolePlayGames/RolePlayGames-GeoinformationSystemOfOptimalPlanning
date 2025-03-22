using GSOP.Domain.Contracts.Customers.Models;
using GSOP.Domain.Contracts.Locations;

namespace GSOP.Domain.Contracts.Customers;

/// <summary>
/// Customer domain model
/// </summary>
public interface ICustomer
{
    CustomerName Name { get; }

    Coordinates? Coordinates { get; }

    Task SetName(CustomerName name);

    void SetCoordinates(Coordinates? coordinates);
}

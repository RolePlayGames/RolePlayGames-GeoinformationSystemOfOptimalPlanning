using GSOP.Domain.Contracts.Orders.Models;

namespace GSOP.Domain.Contracts.Orders.Exceptions;

/// <summary>
/// Represents order's customer does not exists
/// </summary>
public class CustomerDoesNotExistsException : Exception
{
    public long CustomerID { get; }

    public CustomerDoesNotExistsException(CustomerID customerID) : base($"Order's customer {customerID} should exists but does not")
    {
        CustomerID = customerID;
    }
}

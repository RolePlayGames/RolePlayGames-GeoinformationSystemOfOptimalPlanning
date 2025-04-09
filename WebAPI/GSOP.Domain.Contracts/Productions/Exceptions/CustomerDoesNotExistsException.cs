using GSOP.Domain.Contracts.Orders.Models;

namespace GSOP.Domain.Contracts.Productions.Exceptions;

/// <summary>
/// Represents route's customer does not exists
/// </summary>
public class CustomerDoesNotExistsException : Exception
{
    public long CustomerID { get; }

    public CustomerDoesNotExistsException(CustomerID customerID) : base($"Route's customer {customerID} should exists but does not")
    {
        CustomerID = customerID;
    }
}

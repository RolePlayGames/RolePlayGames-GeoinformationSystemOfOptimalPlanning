namespace GSOP.Domain.Contracts.Orders.Exceptions;

/// <summary>
/// Represents order was not found by ID
/// </summary>
public class OrderWasNotFoundException : Exception
{
    public long ID { get; }

    public OrderWasNotFoundException(ID id) : base($"Order was not found by ID {id}")
    {
        ID = id;
    }
}

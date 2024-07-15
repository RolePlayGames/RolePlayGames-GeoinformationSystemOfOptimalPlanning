using GSOP.Domain.Contracts.Orders.Models;

namespace GSOP.Domain.Contracts.Orders.Exceptions;

/// <summary>
/// Represents order number is not unique
/// </summary>
public class OrderNumberAlreadyExistsException : Exception
{
    public string OrderNumber { get; }

    public OrderNumberAlreadyExistsException(OrderNumber orderNumber) : base($"Order number {orderNumber} should be unique but already exists")
    {
        OrderNumber = orderNumber;
    }
}

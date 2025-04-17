namespace GSOP.Application.Contracts.Optimization.Models;

public record OrderRouteInfo
{
    public required string OrderNumber { get; init; }

    public required DateTime OrderDeliveryStartDateTime { get; init; }

    public required DateTime OrderDeliveryEndDateTime { get; init; }
}

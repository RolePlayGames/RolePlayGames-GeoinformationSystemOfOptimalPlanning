namespace GSOP.Application.Contracts.ProductionData.Models;

public record RouteModel
{
    public required string ProductionName { get; init; }

    public required string CustomerName { get; init; }

    public required double Price { get; init; }

    public required int DrivingTimeMinutes { get; init; }
}

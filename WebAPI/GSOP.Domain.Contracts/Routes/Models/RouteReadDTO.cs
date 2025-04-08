namespace GSOP.Domain.Contracts.Routes.Models;

public record RouteReadDTO
{
    public required EntityLocationInfo ProductionInfo { get; init; }

    public required EntityLocationInfo CustomerInfo { get; init; }

    public required double Price { get; init; }

    public required TimeSpan DrivingTime { get; init; }
}

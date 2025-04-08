namespace GSOP.Domain.Contracts.Routes.Models;

public record RouteWriteDTO
{
    public required double Price { get; init; }

    public required TimeSpan DrivingTime { get; init; }
}

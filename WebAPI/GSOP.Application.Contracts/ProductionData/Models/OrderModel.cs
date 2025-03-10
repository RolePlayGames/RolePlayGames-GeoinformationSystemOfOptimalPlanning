namespace GSOP.Application.Contracts.ProductionData.Models;

public record OrderModel
{
    public required string Number { get; init; }

    public required string CustomerName { get; init; }

    public required string FilmRecipeName { get; init; }

    public required int Width { get; init; }

    public required int QuantityInRunningMeter { get; init; }

    public required double FinishedGoods { get; init; }

    public required double Waste { get; init; }

    public required int RollsCount { get; init; }

    public required DateTime? PlannedDate { get; init; }

    public required double PriceOverdue { get; init; }
}

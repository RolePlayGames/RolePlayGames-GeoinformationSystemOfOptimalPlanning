﻿namespace GSOP.Domain.Contracts.Orders.Models;

public record OrderDTO
{
    public required string Number { get; init; }

    public required long CustomerID { get; init; }

    public required long FilmRecipeID { get; init; }

    public required int Width { get; init; }

    public required int QuantityInRunningMeter { get; init; }

    public required double FinishedGoods { get; init; }

    public required double Waste { get; init; }

    public required int RollsCount { get; init; }

    public required DateTime? PlannedDate { get; init; }

    public required double PriceOverdue { get; init; }
}

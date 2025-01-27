namespace GSOP.Domain.Contracts.ProductionLines.ProductionRules;

public record FilmTypeChangeRuleDTO
{
    public required long FilmRecipeFromID { get; init; }

    public required long FilmRecipeToID { get; init; }

    public required TimeSpan ChangeTime { get; init; }

    public required double ChangeConsumption { get; init; }
}

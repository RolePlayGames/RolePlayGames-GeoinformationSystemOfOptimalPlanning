namespace GSOP.Application.Contracts.ProductionData.Models.ChangeRules;

public record FilmTypeChangeRuleModel
{
    public required string ProductionLineName { get; init; }

    public required string FilmRecipeFromArticle { get; init; }

    public required string FilmRecipeToArticle { get; init; }

    public required int ChangeTimeMinutes { get; init; }

    public required double ChangeConsumption { get; init; }
}

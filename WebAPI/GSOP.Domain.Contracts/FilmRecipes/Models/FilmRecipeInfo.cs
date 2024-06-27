namespace GSOP.Domain.Contracts.FilmRecipes.Models;

public record FilmRecipeInfo
{
    public required long ID { get; init; }

    public required string Name { get; init; }
}

using GSOP.Domain.Contracts.FilmRecipes.Models;

namespace GSOP.Domain.Contracts.ProductionLines.Models;

public class NozzleChangeRule
{
    public required FilmRecipeNozzle NozzleTo { get; init; }

    public required ProductionLineChangeValueRule ChangeValueRule { get; init; }
}

using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.Contracts.ProductionLines.ProductionRules;

public class NozzleChangeRule
{
    public required FilmRecipeNozzle NozzleTo { get; init; }

    public required ProductionLineChangeValueRule ChangeValueRule { get; init; }
}

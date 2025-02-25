using GSOP.Domain.Contracts.FilmRecipes.Models;

namespace GSOP.Domain.Contracts.ProductionLines.Models;

public class CoolingLipChangeRule
{
    public required FilmRecipeCoolingLip CoolingLipTo { get; init; }

    public required ProductionLineChangeValueRule ChangeValueRule { get; init; }
}

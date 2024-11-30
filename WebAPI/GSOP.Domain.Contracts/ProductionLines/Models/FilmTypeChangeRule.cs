using GSOP.Domain.Contracts.Orders.Models;
using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.Contracts.ProductionLines.ProductionRules;

public class FilmTypeChangeRule
{
    public required FilmRecipeID FilmRecipeFromID { get; init; }

    public required FilmRecipeID FilmRecipeToID { get; init; }

    public required ProductionLineChangeValueRule ChangeValueRule { get; init; }
}

using GSOP.Domain.Contracts.Orders.Models;

namespace GSOP.Domain.Contracts.ProductionLines.Models;

public class FilmTypeChangeRule
{
    public required FilmRecipeID FilmRecipeFromID { get; init; }

    public required FilmRecipeID FilmRecipeToID { get; init; }

    public required ProductionLineChangeValueRule ChangeValueRule { get; init; }
}

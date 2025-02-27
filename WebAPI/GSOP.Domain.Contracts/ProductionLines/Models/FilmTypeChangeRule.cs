using GSOP.Domain.Contracts.FilmRecipes.Models;

namespace GSOP.Domain.Contracts.ProductionLines.Models;

public class FilmTypeChangeRule
{
    public required FilmTypeID FilmTypeFromID { get; init; }

    public required FilmTypeID FilmTypeToID { get; init; }

    public required ProductionLineChangeValueRule ChangeValueRule { get; init; }
}

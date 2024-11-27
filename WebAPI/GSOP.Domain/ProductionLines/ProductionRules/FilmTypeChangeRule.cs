using GSOP.Domain.Contracts.Orders.Models;
using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.Contracts.ProductionLines.ProductionRules;

public class FilmTypeChangeRule : IFilmTypeChangeRule
{
    public ProductionLineID ProductionLineID { get; private set; }

    public FilmRecipeID FilmRecipeFromID { get; private set; }

    public FilmRecipeID FilmRecipeToID { get; private set; }

    public ProductionLineChangeValueRule ChangeValueRule { get; private set; }
}

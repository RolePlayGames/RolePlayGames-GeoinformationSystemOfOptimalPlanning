using GSOP.Domain.Contracts.Orders.Models;
using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.Contracts.ProductionLines.ProductionRules;

public interface IFilmTypeChangeRule
{
    ProductionLineID ProductionLineID { get; }

    FilmRecipeID FilmRecipeFromID { get; }

    FilmRecipeID FilmRecipeToID { get; }

    ProductionLineChangeValueRule ChangeValueRule { get; }
}

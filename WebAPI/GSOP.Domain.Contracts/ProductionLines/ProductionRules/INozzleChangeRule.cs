using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.Contracts.ProductionLines.ProductionRules;

public interface INozzleChangeRule
{
    ProductionLineID ProductionLineID { get; }

    FilmRecipeNozzle NozzleTo { get; }

    ProductionLineChangeValueRule ChangeValueRule { get; }
}

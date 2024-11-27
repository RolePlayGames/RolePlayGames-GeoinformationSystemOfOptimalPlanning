using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.Contracts.ProductionLines.ProductionRules
{
    public interface ICoolingLipChangeRule
    {
        ProductionLineID ProductionLineID { get; }

        FilmRecipeCoolingLip CoolingLipTo { get; }

        ProductionLineChangeValueRule ChangeValueRule { get; }
    }
}

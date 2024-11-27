using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.Contracts.ProductionLines.ProductionRules;

public class CoolingLipChangeRule : ICoolingLipChangeRule
{
    public ProductionLineID ProductionLineID { get; private set; }

    public FilmRecipeCoolingLip CoolingLipTo { get; private set; }

    public ProductionLineChangeValueRule ChangeValueRule { get; private set; }
}

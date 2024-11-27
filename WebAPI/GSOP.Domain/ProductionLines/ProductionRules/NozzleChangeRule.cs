using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.Contracts.ProductionLines.ProductionRules;

public class NozzleChangeRule : INozzleChangeRule
{
    public ProductionLineID ProductionLineID { get; private set; }

    public FilmRecipeNozzle NozzleTo { get; private set; }

    public ProductionLineChangeValueRule ChangeValueRule { get; private set; }
}

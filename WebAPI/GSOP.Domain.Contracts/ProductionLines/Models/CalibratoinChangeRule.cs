using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.Contracts.ProductionLines.ProductionRules;

public class CalibratoinChangeRule
{
    public required FilmRecipeCalibration CalibrationTo { get; init; }

    public required ProductionLineChangeValueRule ChangeValueRule { get; init; }
}

using GSOP.Domain.Contracts.FilmRecipes.Models;

namespace GSOP.Domain.Contracts.ProductionLines.Models;

public class CalibratoinChangeRule
{
    public required FilmRecipeCalibration CalibrationTo { get; init; }

    public required ProductionLineChangeValueRule ChangeValueRule { get; init; }
}

using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.Contracts.ProductionLines.ProductionRules;

public class CalibratoinChangeRule : ICalibratoinChangeRule
{
    public ProductionLineID ProductionLineID { get; private set; }

    public FilmRecipeCalibration CalibrationTo { get; private set; }

    public ProductionLineChangeValueRule ChangeValueRule { get; private set; }
}

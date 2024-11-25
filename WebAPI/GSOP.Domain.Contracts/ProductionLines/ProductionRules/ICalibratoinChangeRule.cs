using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.Contracts.ProductionLines.ProductionRules;

public interface ICalibratoinChangeRule
{
    ProductionLineID ProductionLineID { get; }

    FilmRecipeCalibration CalibrationTo { get; }

    ProductionLineChangeValueRule ChangeValueRule { get; }
}

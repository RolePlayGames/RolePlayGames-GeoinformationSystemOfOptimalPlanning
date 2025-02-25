using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.FilmRecipes;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Optimization.TargetFunctionCalculators.Cost.Base;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Cost;

public class OrdersReconfigurationCostCalculator : IOrdersReconfigurationCostCalculator
{
    private const double _epsilon = 1e-9;

    private readonly IReadOnlyDictionary<IProductionLine, IReadOnlyDictionary<FilmTypeID, IReadOnlyDictionary<FilmTypeID, double>>> _filmTypeChanges = new Dictionary<IProductionLine, IReadOnlyDictionary<FilmTypeID, IReadOnlyDictionary<FilmTypeID, double>>>();
    private readonly IReadOnlyDictionary<IProductionLine, IReadOnlyDictionary<FilmRecipeCoolingLip, IReadOnlyDictionary<FilmRecipeCoolingLip, double>>> _coolingLipChanges = new Dictionary<IProductionLine, IReadOnlyDictionary<FilmRecipeCoolingLip, IReadOnlyDictionary<FilmRecipeCoolingLip, double>>>();
    private readonly IReadOnlyDictionary<IProductionLine, IReadOnlyDictionary<FilmRecipeCalibration, IReadOnlyDictionary<FilmRecipeCalibration, double>>> _calibrationChanges = new Dictionary<IProductionLine, IReadOnlyDictionary<FilmRecipeCalibration, IReadOnlyDictionary<FilmRecipeCalibration, double>>>();
    private readonly IReadOnlyDictionary<IProductionLine, IReadOnlyDictionary<FilmRecipeNozzle, IReadOnlyDictionary<FilmRecipeNozzle, double>>> _nozzleChanges = new Dictionary<IProductionLine, IReadOnlyDictionary<FilmRecipeNozzle, IReadOnlyDictionary<FilmRecipeNozzle, double>>>();

    private readonly IDictionary<IOrder, IDictionary<IOrder, double>> _ordersReconfiguration = new Dictionary<IOrder, IDictionary<IOrder, double>>();

    public double Calculate(IProductionLine productionLine, IOrder orderFrom, IOrder orderTo)
    {
        if (_ordersReconfiguration.TryGetValue(orderFrom, out var orderReconfiguration) && orderReconfiguration.TryGetValue(orderTo, out var reconfiguration))
            return reconfiguration;

        double result = 0;

        if (orderFrom.FilmRecipe.FilmTypeID != orderTo.FilmRecipe.FilmTypeID)
        {
            result += _filmTypeChanges[productionLine].TryGetValue(orderFrom.FilmRecipe.FilmTypeID, out var changes) ? changes.TryGetValue(orderTo.FilmRecipe.FilmTypeID, out var change) ? change : 0 : 0;
        }

        if (AreNotEqual(orderFrom.FilmRecipe.CoolingLip, orderTo.FilmRecipe.CoolingLip))
        {
            result += _coolingLipChanges[productionLine].TryGetValue(orderFrom.FilmRecipe.CoolingLip, out var changes) ? changes.TryGetValue(orderTo.FilmRecipe.CoolingLip, out var change) ? change : 0 : 0;
        }

        if (AreNotEqual(orderFrom.FilmRecipe.Calibration, orderTo.FilmRecipe.Calibration))
        {
            result += _calibrationChanges[productionLine].TryGetValue(orderFrom.FilmRecipe.Calibration, out var changes) ? changes.TryGetValue(orderTo.FilmRecipe.Calibration, out var change) ? change : 0 : 0;
        }

        if (AreNotEqual(orderFrom.FilmRecipe.Nozzle, orderTo.FilmRecipe.Nozzle))
        {
            result += _nozzleChanges[productionLine].TryGetValue(orderFrom.FilmRecipe.Nozzle, out var changes) ? changes.TryGetValue(orderTo.FilmRecipe.Nozzle, out var change) ? change : 0 : 0;
        }

        if (AreNotEqual(orderFrom.Width, orderTo.Width))
        {
            result += productionLine.WidthChangeRule.ChangeConsumption;
        }

        if (AreNotEqual(orderFrom.FilmRecipe.Thickness, orderTo.FilmRecipe.Thickness))
        {
            result += productionLine.ThicknessChangeRule.ChangeConsumption;
        }

        if (_ordersReconfiguration.TryGetValue(orderFrom, out var reconfigurations))
        {
            reconfigurations.Add(orderTo, result);
        }
        else
        {
            _ordersReconfiguration.Add(orderFrom, new Dictionary<IOrder, double> { { orderTo, result } });
        }

        return result;
    }

    private static bool AreNotEqual(double value1, double value2)
    {
        var difference = Math.Abs(value1 * _epsilon);

        return Math.Abs(value1 - value2) <= difference;
    }
}

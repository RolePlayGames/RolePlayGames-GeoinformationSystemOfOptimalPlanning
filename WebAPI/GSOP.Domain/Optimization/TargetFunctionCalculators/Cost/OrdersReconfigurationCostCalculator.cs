using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Cost;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using System.Collections.Concurrent;
using System.Collections.Frozen;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Cost;

public class OrdersReconfigurationCostCalculator : IOrdersReconfigurationCostCalculator
{
    private const double _epsilon = 1e-9;

    private readonly ConcurrentDictionary<IProductionLine, FrozenDictionary<FilmTypeID, FrozenDictionary<FilmTypeID, double>>> _filmTypeChanges = [];
    private readonly ConcurrentDictionary<IProductionLine, FrozenDictionary<FilmRecipeCoolingLip, double>> _coolingLipChanges = [];
    private readonly ConcurrentDictionary<IProductionLine, FrozenDictionary<FilmRecipeCalibration, double>> _calibrationChanges = [];
    private readonly ConcurrentDictionary<IProductionLine, FrozenDictionary<FilmRecipeNozzle, double>> _nozzleChanges = [];

    private readonly ConcurrentDictionary<IOrder, Dictionary<IOrder, double>> _ordersReconfiguration = [];

    public double Calculate(IProductionLine productionLine, IOrder orderFrom, IOrder orderTo)
    {
        if (_ordersReconfiguration.TryGetValue(orderFrom, out var orderReconfiguration) && orderReconfiguration.TryGetValue(orderTo, out var reconfiguration))
            return reconfiguration;

        double result = 0;

        if (orderFrom.FilmRecipe.FilmTypeID != orderTo.FilmRecipe.FilmTypeID)
        {
            if (!_filmTypeChanges.TryGetValue(productionLine, out var value))
            {
                _filmTypeChanges[productionLine] = value = productionLine.FilmTypeChangeRules.GroupBy(x => x.FilmTypeFromID).ToFrozenDictionary(x => x.Key, x => x.ToFrozenDictionary(x => x.FilmTypeToID, x => x.ChangeValueRule.ChangeConsumption));
            }

            result += value.TryGetValue(orderFrom.FilmRecipe.FilmTypeID, out var changes) ? changes.TryGetValue(orderTo.FilmRecipe.FilmTypeID, out var change) ? change : 0 : 0;
        }

        if (AreNotEqual(orderFrom.FilmRecipe.CoolingLip, orderTo.FilmRecipe.CoolingLip))
        {
            if (!_coolingLipChanges.TryGetValue(productionLine, out var value))
            {
                _coolingLipChanges[productionLine] = value = productionLine.CoolingLipChangeRules.ToFrozenDictionary(x => x.CoolingLipTo, x => x.ChangeValueRule.ChangeConsumption);
            }

            result += value.TryGetValue(orderTo.FilmRecipe.CoolingLip, out var change) ? change : 0;
        }

        if (AreNotEqual(orderFrom.FilmRecipe.Calibration, orderTo.FilmRecipe.Calibration))
        {
            if (!_calibrationChanges.TryGetValue(productionLine, out var value))
            {
                _calibrationChanges[productionLine] = value = productionLine.CalibratoinChangeRules.ToFrozenDictionary(x => x.CalibrationTo, x => x.ChangeValueRule.ChangeConsumption);
            }

            result += value.TryGetValue(orderTo.FilmRecipe.Calibration, out var change) ? change : 0;
        }

        if (AreNotEqual(orderFrom.FilmRecipe.Nozzle, orderTo.FilmRecipe.Nozzle))
        {
            if (!_nozzleChanges.TryGetValue(productionLine, out var value))
            {
                _nozzleChanges[productionLine] = value = productionLine.NozzleChangeRules.ToFrozenDictionary(x => x.NozzleTo, x => x.ChangeValueRule.ChangeConsumption);
            }

            result += value.TryGetValue(orderTo.FilmRecipe.Nozzle, out var change) ? change : 0;
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
            _ordersReconfiguration.TryAdd(orderFrom, new Dictionary<IOrder, double> { { orderTo, result } });
        }

        return result;
    }

    private static bool AreNotEqual(double value1, double value2)
    {
        var difference = Math.Abs(value1 * _epsilon);

        return Math.Abs(value1 - value2) <= difference;
    }
}

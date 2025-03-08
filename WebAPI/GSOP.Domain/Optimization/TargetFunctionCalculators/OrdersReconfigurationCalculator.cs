using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Cost;
using GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Time;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Contracts.ProductionLines.Models;
using System.Collections.Concurrent;
using System.Collections.Frozen;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators;

public class OrdersReconfigurationCalculator : IOrdersReconfigurationTimeCalculator, IOrdersReconfigurationCostCalculator
{
    private const double _epsilon = 1e-9;

    private readonly ConcurrentDictionary<IProductionLine, IDictionary<FilmTypeID, FrozenDictionary<FilmTypeID, ProductionLineChangeValueRule>>> _filmTypeChanges = [];
    private readonly ConcurrentDictionary<IProductionLine, IDictionary<FilmRecipeCoolingLip, ProductionLineChangeValueRule>> _coolingLipChanges = [];
    private readonly ConcurrentDictionary<IProductionLine, IDictionary<FilmRecipeCalibration, ProductionLineChangeValueRule>> _calibrationChanges = [];
    private readonly ConcurrentDictionary<IProductionLine, IDictionary<FilmRecipeNozzle, ProductionLineChangeValueRule>> _nozzleChanges = [];

    private readonly ConcurrentDictionary<IOrder, ConcurrentDictionary<IOrder, double>> _ordersReconfigurationCost = [];
    private readonly ConcurrentDictionary<IOrder, ConcurrentDictionary<IOrder, double>> _ordersReconfigurationTime = [];

    double IOrdersReconfigurationTimeCalculator.Calculate(IProductionLine productionLine, IOrder orderFrom, IOrder orderTo)
    {
        var orderReconfiguration = _ordersReconfigurationTime.GetOrAdd(orderFrom, (order) => new ConcurrentDictionary<IOrder, double>());

        return orderReconfiguration.GetOrAdd(orderTo, (order) =>
        {
            var result = TimeSpan.Zero;

            foreach (var change in GetChanges(productionLine, orderFrom, orderTo))
            {
                result += change.ChangeTime;
            }

            return result.TotalMinutes;
        });
    }

    double IOrdersReconfigurationCostCalculator.Calculate(IProductionLine productionLine, IOrder orderFrom, IOrder orderTo)
    {
        var orderReconfiguration = _ordersReconfigurationCost.GetOrAdd(orderFrom, (order) => new ConcurrentDictionary<IOrder, double>());

        return orderReconfiguration.GetOrAdd(orderTo, (order) =>
        {
            var result = 0.0;

            foreach (var change in GetChanges(productionLine, orderFrom, orderTo))
            {
                result += change.ChangeConsumption;
            }

            return result;
        });
    }

    private IEnumerable<ProductionLineChangeValueRule> GetChanges(IProductionLine productionLine, IOrder orderFrom, IOrder orderTo)
    {
        if (orderFrom.FilmRecipe.FilmTypeID != orderTo.FilmRecipe.FilmTypeID)
        {
            var filmTypeChanges = _filmTypeChanges.GetOrAdd(productionLine, pl => pl.FilmTypeChangeRules.GroupBy(x => x.FilmTypeFromID).ToFrozenDictionary(x => x.Key,x => x.ToFrozenDictionary(y => y.FilmTypeToID, y => y.ChangeValueRule)));

            if (filmTypeChanges.TryGetValue(orderFrom.FilmRecipe.FilmTypeID, out var changes) && changes.TryGetValue(orderTo.FilmRecipe.FilmTypeID, out var change))
            {
                yield return change;
            }
        }

        if (AreNotEqual(orderFrom.FilmRecipe.CoolingLip, orderTo.FilmRecipe.CoolingLip))
        {
            var coolingLipChanges = _coolingLipChanges.GetOrAdd(productionLine, pl => pl.CoolingLipChangeRules.ToFrozenDictionary(x => x.CoolingLipTo, x => x.ChangeValueRule));

            if (coolingLipChanges.TryGetValue(orderTo.FilmRecipe.CoolingLip, out var change))
            {
                yield return change;
            }
        }

        if (AreNotEqual(orderFrom.FilmRecipe.Calibration, orderTo.FilmRecipe.Calibration))
        {
            var calibrationChanges = _calibrationChanges.GetOrAdd(productionLine, pl => pl.CalibratoinChangeRules.ToFrozenDictionary(x => x.CalibrationTo, x => x.ChangeValueRule));

            if (calibrationChanges.TryGetValue(orderTo.FilmRecipe.Calibration, out var change))
            {
                yield return change;
            }
        }

        if (AreNotEqual(orderFrom.FilmRecipe.Nozzle, orderTo.FilmRecipe.Nozzle))
        {
            var nozzleChanges = _nozzleChanges.GetOrAdd(productionLine, pl => pl.NozzleChangeRules.ToFrozenDictionary(x => x.NozzleTo, x => x.ChangeValueRule));

            if (nozzleChanges.TryGetValue(orderTo.FilmRecipe.Nozzle, out var change))
            {
                yield return change;
            }
        }

        if (AreNotEqual(orderFrom.Width, orderTo.Width))
        {
            yield return productionLine.WidthChangeRule;
        }

        if (AreNotEqual(orderFrom.FilmRecipe.Thickness, orderTo.FilmRecipe.Thickness))
        {
            yield return productionLine.ThicknessChangeRule;
        }
    }

    private static bool AreNotEqual(double value1, double value2)
    {
        var difference = Math.Abs(value1 * _epsilon);
        return Math.Abs(value1 - value2) > difference;
    }
}

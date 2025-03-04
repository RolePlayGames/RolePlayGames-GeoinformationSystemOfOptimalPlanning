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

    private readonly object _filmTypeChangesLock = new();
    private readonly object _coolingLipChangesLock = new();
    private readonly object _calibrationChangesLock = new();
    private readonly object _nozzleChangesLock = new();

    private readonly ConcurrentDictionary<IOrder, ConcurrentDictionary<IOrder, double>> _ordersReconfigurationCost = [];
    private readonly ConcurrentDictionary<IOrder, ConcurrentDictionary<IOrder, double>> _ordersReconfigurationTime = [];

    double IOrdersReconfigurationTimeCalculator.Calculate(IProductionLine productionLine, IOrder orderFrom, IOrder orderTo)
    {
        if (_ordersReconfigurationTime.TryGetValue(orderFrom, out var orderReconfiguration) && orderReconfiguration.TryGetValue(orderTo, out var reconfiguration))
            return reconfiguration;

        var result = TimeSpan.Zero;

        foreach (var change in GetChanges(productionLine, orderFrom, orderTo))
        {
            result += change.ChangeTime;
        }

        var resultMinutes = result.TotalMinutes;

        if (_ordersReconfigurationTime.TryGetValue(orderFrom, out var reconfigurations))
        {
            reconfigurations.TryAdd(orderTo, resultMinutes);
        }
        else
        {
            var newOrderReconfiguration = new ConcurrentDictionary<IOrder, double>();
            newOrderReconfiguration.TryAdd(orderTo, resultMinutes);
            _ordersReconfigurationTime.TryAdd(orderFrom, newOrderReconfiguration);
        }

        return resultMinutes;
    }

    double IOrdersReconfigurationCostCalculator.Calculate(IProductionLine productionLine, IOrder orderFrom, IOrder orderTo)
    {
        if (_ordersReconfigurationCost.TryGetValue(orderFrom, out var orderReconfiguration) && orderReconfiguration.TryGetValue(orderTo, out var reconfiguration))
            return reconfiguration;

        var result = 0.0;

        foreach (var change in GetChanges(productionLine, orderFrom, orderTo))
        {
            result += change.ChangeConsumption;
        }

        if (_ordersReconfigurationCost.TryGetValue(orderFrom, out var reconfigurations))
        {
            reconfigurations.TryAdd(orderTo, result);
        }
        else
        {
            var newOrderReconfiguration = new ConcurrentDictionary<IOrder, double>();
            newOrderReconfiguration.TryAdd(orderTo, result);
            _ordersReconfigurationCost.TryAdd(orderFrom, newOrderReconfiguration);
        }

        return result;
    }

    private IEnumerable<ProductionLineChangeValueRule> GetChanges(IProductionLine productionLine, IOrder orderFrom, IOrder orderTo)
    {
        if (orderFrom.FilmRecipe.FilmTypeID != orderTo.FilmRecipe.FilmTypeID)
        {
            if (!_filmTypeChanges.TryGetValue(productionLine, out var value))
            {
                lock (_filmTypeChangesLock)
                {
                    _filmTypeChanges.TryAdd(productionLine, value = productionLine.FilmTypeChangeRules.GroupBy(x => x.FilmTypeFromID).ToFrozenDictionary(x => x.Key, x => x.ToFrozenDictionary(x => x.FilmTypeToID, x => x.ChangeValueRule)));
                }
            }

            if (_filmTypeChanges[productionLine].TryGetValue(orderFrom.FilmRecipe.FilmTypeID, out var changes) && changes.TryGetValue(orderTo.FilmRecipe.FilmTypeID, out var change))
                yield return change;
        }

        if (AreNotEqual(orderFrom.FilmRecipe.CoolingLip, orderTo.FilmRecipe.CoolingLip))
        {
            if (!_coolingLipChanges.TryGetValue(productionLine, out var value))
            {
                lock (_coolingLipChangesLock)
                {
                    _coolingLipChanges.TryAdd(productionLine, value = productionLine.CoolingLipChangeRules.ToFrozenDictionary(x => x.CoolingLipTo, x => x.ChangeValueRule));
                }
            }

            if (_coolingLipChanges[productionLine].TryGetValue(orderTo.FilmRecipe.CoolingLip, out var change))
                yield return change;
        }

        if (AreNotEqual(orderFrom.FilmRecipe.Calibration, orderTo.FilmRecipe.Calibration))
        {
            if (!_calibrationChanges.TryGetValue(productionLine, out var value))
            {
                lock (_calibrationChangesLock)
                {
                    _calibrationChanges.TryAdd(productionLine, value = productionLine.CalibratoinChangeRules.ToFrozenDictionary(x => x.CalibrationTo, x => x.ChangeValueRule));
                }
            }

            if (_calibrationChanges[productionLine].TryGetValue(orderTo.FilmRecipe.Calibration, out var change))
                yield return change;
        }

        if (AreNotEqual(orderFrom.FilmRecipe.Nozzle, orderTo.FilmRecipe.Nozzle))
        {
            if (!_nozzleChanges.TryGetValue(productionLine, out var value))
            {
                lock (_nozzleChangesLock)
                {
                    _nozzleChanges.TryAdd(productionLine, value = productionLine.NozzleChangeRules.ToFrozenDictionary(x => x.NozzleTo, x => x.ChangeValueRule));
                }
            }

            if (_nozzleChanges[productionLine].TryGetValue(orderTo.FilmRecipe.Nozzle, out var change))
                yield return change;
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

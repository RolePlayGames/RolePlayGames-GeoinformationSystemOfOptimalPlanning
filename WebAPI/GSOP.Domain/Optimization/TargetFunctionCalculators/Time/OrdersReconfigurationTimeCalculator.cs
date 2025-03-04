using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Time;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using System.Collections.Concurrent;
using System.Collections.Frozen;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Time;

public class OrdersReconfigurationTimeCalculator : IOrdersReconfigurationTimeCalculator
{
    private const double _epsilon = 1e-9;

    private readonly ConcurrentDictionary<IProductionLine, IDictionary<FilmTypeID, FrozenDictionary<FilmTypeID, TimeSpan>>> _filmTypeChanges = [];
    private readonly ConcurrentDictionary<IProductionLine, IDictionary<FilmRecipeCoolingLip, TimeSpan>> _coolingLipChanges = [];
    private readonly ConcurrentDictionary<IProductionLine, IDictionary<FilmRecipeCalibration, TimeSpan>> _calibrationChanges = [];
    private readonly ConcurrentDictionary<IProductionLine, IDictionary<FilmRecipeNozzle, TimeSpan>> _nozzleChanges = [];

    private readonly object _filmTypeChangesLock = new();
    private readonly object _coolingLipChangesLock = new();
    private readonly object _calibrationChangesLock = new();
    private readonly object _nozzleChangesLock = new();

    private readonly ConcurrentDictionary<IOrder, ConcurrentDictionary<IOrder, double>> _ordersReconfiguration = [];

    public double Calculate(IProductionLine productionLine, IOrder orderFrom, IOrder orderTo)
    {
        if (_ordersReconfiguration.TryGetValue(orderFrom, out var orderReconfiguration) && orderReconfiguration.TryGetValue(orderTo, out var reconfiguration))
            return reconfiguration;

        var result = TimeSpan.Zero;

        if (orderFrom.FilmRecipe.FilmTypeID != orderTo.FilmRecipe.FilmTypeID)
        {
            if (!_filmTypeChanges.TryGetValue(productionLine, out var value))
            {
                lock(_filmTypeChangesLock)
                {
                    _filmTypeChanges.TryAdd(productionLine, value = productionLine.FilmTypeChangeRules.GroupBy(x => x.FilmTypeFromID).ToFrozenDictionary(x => x.Key, x => x.ToFrozenDictionary(x => x.FilmTypeToID, x => x.ChangeValueRule.ChangeTime)));
                }
            }

            result += _filmTypeChanges[productionLine].TryGetValue(orderFrom.FilmRecipe.FilmTypeID, out var changes) ? changes.TryGetValue(orderTo.FilmRecipe.FilmTypeID, out var change) ? change : TimeSpan.Zero : TimeSpan.Zero;
        }

        if (AreNotEqual(orderFrom.FilmRecipe.CoolingLip, orderTo.FilmRecipe.CoolingLip))
        {
            if (!_coolingLipChanges.TryGetValue(productionLine, out var value))
            {
                lock (_coolingLipChangesLock)
                {
                    _coolingLipChanges.TryAdd(productionLine, value = productionLine.CoolingLipChangeRules.ToFrozenDictionary(x => x.CoolingLipTo, x => x.ChangeValueRule.ChangeTime));
                }
            }

            result += _coolingLipChanges[productionLine].TryGetValue(orderTo.FilmRecipe.CoolingLip, out var change) ? change : TimeSpan.Zero;
        }

        if (AreNotEqual(orderFrom.FilmRecipe.Calibration, orderTo.FilmRecipe.Calibration))
        {
            if (!_calibrationChanges.TryGetValue(productionLine, out var value))
            {
                lock (_calibrationChangesLock)
                {
                    _calibrationChanges.TryAdd(productionLine, value = productionLine.CalibratoinChangeRules.ToFrozenDictionary(x => x.CalibrationTo, x => x.ChangeValueRule.ChangeTime));
                }
            }

            result += _calibrationChanges[productionLine].TryGetValue(orderTo.FilmRecipe.Calibration, out var change) ? change : TimeSpan.Zero;
        }

        if (AreNotEqual(orderFrom.FilmRecipe.Nozzle, orderTo.FilmRecipe.Nozzle))
        {
            if (!_nozzleChanges.TryGetValue(productionLine, out var value))
            {
                lock (_nozzleChangesLock)
                {
                    _nozzleChanges.TryAdd(productionLine, value = productionLine.NozzleChangeRules.ToFrozenDictionary(x => x.NozzleTo, x => x.ChangeValueRule.ChangeTime));
                }
            }

            result += _nozzleChanges[productionLine].TryGetValue(orderTo.FilmRecipe.Nozzle, out var change) ? change : TimeSpan.Zero;
        }

        if (AreNotEqual(orderFrom.Width, orderTo.Width))
        {
            result += productionLine.WidthChangeRule.ChangeTime;
        }

        if (AreNotEqual(orderFrom.FilmRecipe.Thickness, orderTo.FilmRecipe.Thickness))
        {
            result += productionLine.ThicknessChangeRule.ChangeTime;
        }

        var resultTime = result.TotalMinutes;

        if (_ordersReconfiguration.TryGetValue(orderFrom, out var reconfigurations))
        {
            reconfigurations.TryAdd(orderTo, resultTime);
        }
        else
        {
            var newOrderReconfiguration = new ConcurrentDictionary<IOrder, double>();
            newOrderReconfiguration.TryAdd(orderTo, resultTime);
            _ordersReconfiguration.TryAdd(orderFrom, newOrderReconfiguration);
        }

        return resultTime;
    }

    private static bool AreNotEqual(double value1, double value2)
    {
        var difference = Math.Abs(value1 * _epsilon);

        return Math.Abs(value1 - value2) > difference;
    }
}

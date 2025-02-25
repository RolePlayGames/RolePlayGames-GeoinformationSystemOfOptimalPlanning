using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Optimization.TargetFunctionCalculators.Time.Base;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Time;

public class OrdersReconfigurationTimeCalculator : IOrdersReconfigurationTimeCalculator
{
    private const double _epsilon = 1e-9;

    private readonly IReadOnlyDictionary<IProductionLine, IReadOnlyDictionary<FilmTypeID, IReadOnlyDictionary<FilmTypeID, TimeSpan>>> _filmTypeChanges = new Dictionary<IProductionLine, IReadOnlyDictionary<FilmTypeID, IReadOnlyDictionary<FilmTypeID, TimeSpan>>>();
    private readonly IReadOnlyDictionary<IProductionLine, IReadOnlyDictionary<FilmRecipeCoolingLip, IReadOnlyDictionary<FilmRecipeCoolingLip, TimeSpan>>> _coolingLipChanges = new Dictionary<IProductionLine, IReadOnlyDictionary<FilmRecipeCoolingLip, IReadOnlyDictionary<FilmRecipeCoolingLip, TimeSpan>>>();
    private readonly IReadOnlyDictionary<IProductionLine, IReadOnlyDictionary<FilmRecipeCalibration, IReadOnlyDictionary<FilmRecipeCalibration, TimeSpan>>> _calibrationChanges = new Dictionary<IProductionLine, IReadOnlyDictionary<FilmRecipeCalibration, IReadOnlyDictionary<FilmRecipeCalibration, TimeSpan>>>();
    private readonly IReadOnlyDictionary<IProductionLine, IReadOnlyDictionary<FilmRecipeNozzle, IReadOnlyDictionary<FilmRecipeNozzle, TimeSpan>>> _nozzleChanges = new Dictionary<IProductionLine, IReadOnlyDictionary<FilmRecipeNozzle, IReadOnlyDictionary<FilmRecipeNozzle, TimeSpan>>>();

    private readonly IDictionary<IOrder, IDictionary<IOrder, double>> _ordersReconfiguration = new Dictionary<IOrder, IDictionary<IOrder, double>>();

    public double Calculate(IProductionLine productionLine, IOrder orderFrom, IOrder orderTo)
    {
        if (_ordersReconfiguration.TryGetValue(orderFrom, out var orderReconfiguration) && orderReconfiguration.TryGetValue(orderTo, out var reconfiguration))
            return reconfiguration;

        var result = TimeSpan.Zero;

        if (orderFrom.FilmRecipe.FilmTypeID != orderTo.FilmRecipe.FilmTypeID)
        {
            result += _filmTypeChanges[productionLine].TryGetValue(orderFrom.FilmRecipe.FilmTypeID, out var changes) ? changes.TryGetValue(orderTo.FilmRecipe.FilmTypeID, out var change) ? change : TimeSpan.Zero : TimeSpan.Zero;
        }

        if (AreNotEqual(orderFrom.FilmRecipe.CoolingLip, orderTo.FilmRecipe.CoolingLip))
        {
            result += _coolingLipChanges[productionLine].TryGetValue(orderFrom.FilmRecipe.CoolingLip, out var changes) ? changes.TryGetValue(orderTo.FilmRecipe.CoolingLip, out var change) ? change : TimeSpan.Zero : TimeSpan.Zero;
        }

        if (AreNotEqual(orderFrom.FilmRecipe.Calibration, orderTo.FilmRecipe.Calibration))
        {
            result += _calibrationChanges[productionLine].TryGetValue(orderFrom.FilmRecipe.Calibration, out var changes) ? changes.TryGetValue(orderTo.FilmRecipe.Calibration, out var change) ? change : TimeSpan.Zero : TimeSpan.Zero;
        }

        if (AreNotEqual(orderFrom.FilmRecipe.Nozzle, orderTo.FilmRecipe.Nozzle))
        {
            result += _nozzleChanges[productionLine].TryGetValue(orderFrom.FilmRecipe.Nozzle, out var changes) ? changes.TryGetValue(orderTo.FilmRecipe.Nozzle, out var change) ? change : TimeSpan.Zero : TimeSpan.Zero;
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
            reconfigurations.Add(orderTo, resultTime);
        }
        else
        {
            _ordersReconfiguration.Add(orderFrom, new Dictionary<IOrder, double> { { orderTo, resultTime } });
        }

        return resultTime;
    }

    private static bool AreNotEqual(double value1, double value2)
    {
        var difference = Math.Abs(value1 * _epsilon);

        return Math.Abs(value1 - value2) <= difference;
    }
}

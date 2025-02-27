using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Contracts.Optimization.Models;
using GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Time;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.Optimization.Approximation;

public class ApproximationAlgorithm : IOptimizationAlgorithm<ProductionPlan>
{
    private readonly IReadOnlyCollection<IProductionLine> _productionLines;
    private readonly IReadOnlyCollection<IOrder> _orders;
    private readonly IOrderExcecutionTimeCalculator _orderExcecutionTimeCalculator;
    private readonly IReadOnlyDictionary<IProductionLine, IReadOnlyCollection<FilmTypeChangeRule>> _filmTypeChanges;

    public ApproximationAlgorithm(
        IReadOnlyCollection<IProductionLine> productionLines,
        IReadOnlyCollection<IOrder> orders,
        IOrderExcecutionTimeCalculator orderExcecutionTimeCalculator,
        IReadOnlyDictionary<IProductionLine, IReadOnlyCollection<FilmTypeChangeRule>> filmTypeChanges)
    {
        _productionLines = productionLines;
        _orders = orders;
        _orderExcecutionTimeCalculator = orderExcecutionTimeCalculator;
        _filmTypeChanges = filmTypeChanges;
    }

    public ProductionPlan GetResolve()
    {
        var productionLineQueues = new List<ProductionLineQueue>(_productionLines.Count);
        var ordersByTime = _orders.OrderByDescending(order => _orderExcecutionTimeCalculator).ToList();

        for (var i = 0; i < _productionLines.Count; i++)
        {
            var orders = new List<IOrder>();

            for (var j = i; j < ordersByTime.Count; j += _productionLines.Count)
            {
                orders.Add(ordersByTime[j]);
            }

            if (ordersByTime.Count % _productionLines.Count != 0 && ordersByTime.Count % _productionLines.Count() <= i)
            {
                var index = ordersByTime.Count / _productionLines.Count;

                orders.Add(ordersByTime[index + i]);
            }

            productionLineQueues.Add(BestProductionPlan(_productionLines.ElementAt(i), orders));
        }

        return new ProductionPlan { ProductionLineQueues = productionLineQueues };
    }

    private ProductionLineQueue BestProductionPlan(IProductionLine productionLine, List<IOrder> orders)
    {
        var orderedOrders = new List<IOrder>();
        var extruderRecipeChange = _filmTypeChanges[productionLine].OrderByDescending(x => x.ChangeValueRule.ChangeTime).ToList();

        foreach (var change in extruderRecipeChange)
        {
            if (orders.Where(order => order.FilmRecipe.ID == change.FilmTypeToID).Any())
            {
                foreach (var order in orders.Where(or => or.FilmRecipe.ID == change.FilmTypeToID)
                    .OrderBy(order => order.FilmRecipe.Nozzle)
                    .ThenBy(order => order.FilmRecipe.Calibration)
                    .ThenBy(order => order.FilmRecipe.CoolingLip))
                {
                    orderedOrders.Add(order);
                }
            }
        }

        var i = 0;

        while (orderedOrders.Count < orders.Count && i < 2000)
        {
            var lastFilmRecipe = orderedOrders.Last().FilmRecipe;
            var recipeChanges = extruderRecipeChange.Where(change => change.FilmTypeFromID == lastFilmRecipe.ID).OrderBy(change => change.ChangeValueRule.ChangeTime);

            foreach (var change in recipeChanges)
            {
                if (orders.Where(order => order.FilmRecipe.FilmTypeID == change.FilmTypeToID).Any())
                {
                    if (!orderedOrders.Where(x => x.FilmRecipe.FilmTypeID == change.FilmTypeToID).Any())
                    {
                        foreach (var order in orders.Where(or => or.FilmRecipe.FilmTypeID == change.FilmTypeToID)
                            .OrderBy(order => order.FilmRecipe.Nozzle)
                            .ThenBy(order => order.FilmRecipe.Calibration)
                            .ThenBy(order => order.FilmRecipe.CoolingLip))
                        {
                            orderedOrders.Add(order);
                        }

                        break;
                    }
                }
            }

            i++;
        }

        return new() { ProductionLine = productionLine, Orders = orderedOrders };
    }
}

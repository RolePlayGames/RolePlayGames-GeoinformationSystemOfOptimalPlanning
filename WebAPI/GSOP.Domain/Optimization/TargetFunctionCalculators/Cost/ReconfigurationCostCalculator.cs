using GSOP.Domain.Contracts.Optimization.Models;
using GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Cost;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Cost;

public class ReconfigurationCostCalculator : IReconfigurationCostCalculator
{
    private readonly IOrdersReconfigurationCostCalculator _ordersReconfigurationCostCalculator;

    public ReconfigurationCostCalculator(IOrdersReconfigurationCostCalculator ordersReconfigurationCostCalculator)
    {
        _ordersReconfigurationCostCalculator = ordersReconfigurationCostCalculator ?? throw new ArgumentNullException(nameof(ordersReconfigurationCostCalculator));
    }

    public double Calculate(ProductionLineQueue productionLineQueue)
    {
        var result = 0d;

        for (var i = 0; i < productionLineQueue.Orders.Count - 1; i++)
        {
            result += _ordersReconfigurationCostCalculator.Calculate(productionLineQueue.ProductionLine,
                productionLineQueue.Orders.ElementAt(i),
                productionLineQueue.Orders.ElementAt(i + 1));
        }

        return result;
    }
}

using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Contracts.Optimization.Models;
using GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Cost;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Cost;

public class CostFunctionCalculator : ITargetFunctionCalculator<ProductionPlan>
{
    private readonly IProductionLineQueueCostCalculator _productionLineQueueCostCalculator;

    public CostFunctionCalculator(IProductionLineQueueCostCalculator productionLineQueueCostCalculator)
    {
        _productionLineQueueCostCalculator = productionLineQueueCostCalculator ?? throw new ArgumentNullException(nameof(productionLineQueueCostCalculator));
    }

    public double Calculate(ProductionPlan individual)
    {
        return individual.ProductionLineQueues.Sum(_productionLineQueueCostCalculator.Calculate);
    }
}

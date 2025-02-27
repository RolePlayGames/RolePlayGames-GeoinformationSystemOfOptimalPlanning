using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Contracts.Optimization.Models;
using GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Time;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Time;

public class TimeFunctionCalculator : ITargetFunctionCalculator<ProductionPlan>
{
    private readonly IProductionLineQueueTimeCalculator _productionLineQueueTimeCalculator;

    public TimeFunctionCalculator(IProductionLineQueueTimeCalculator productionLineQueueTimeCalculator)
    {
        _productionLineQueueTimeCalculator = productionLineQueueTimeCalculator ?? throw new ArgumentNullException(nameof(productionLineQueueTimeCalculator));
    }

    public double Calculate(ProductionPlan individual)
    {
        return individual.ProductionLineQueues.Max(_productionLineQueueTimeCalculator.Calculate);
    }
}

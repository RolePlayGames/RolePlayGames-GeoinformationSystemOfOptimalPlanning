using GSOP.Domain.Contracts.Optimization.Models;
using GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Time;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Time;

public class ExecutionTimeCalculator : IExecutionTimeCalculator
{
    public IOrderExcecutionTimeCalculator OrderExcecutionTimeCalculator { get; }

    public ExecutionTimeCalculator(IOrderExcecutionTimeCalculator orderExcecutionTimeCalculator)
    {
        OrderExcecutionTimeCalculator = orderExcecutionTimeCalculator ?? throw new ArgumentNullException(nameof(orderExcecutionTimeCalculator));
    }

    public double Calculate(ProductionLineQueue productionLineQueue)
    {
        return productionLineQueue.Orders.Sum(OrderExcecutionTimeCalculator.Calculate);
    }
}

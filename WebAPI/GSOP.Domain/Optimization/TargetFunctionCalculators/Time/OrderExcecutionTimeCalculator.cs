using GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Time;
using GSOP.Domain.Contracts.Orders;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Time;

public class OrderExcecutionTimeCalculator : IOrderExcecutionTimeCalculator
{
    public double Calculate(IOrder order)
    {
        return Convert.ToDouble(order.QuantityInRunningMeter / order.FilmRecipe.ProductionSpeed);
    }
}

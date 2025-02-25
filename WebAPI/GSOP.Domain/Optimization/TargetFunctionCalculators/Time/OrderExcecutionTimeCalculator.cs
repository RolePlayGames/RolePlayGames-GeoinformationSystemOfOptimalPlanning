using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Optimization.TargetFunctionCalculators.Time.Base;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Time;

public class OrderExcecutionTimeCalculator : IOrderExcecutionTimeCalculator
{
    public double Calculate(IOrder order)
    {
        return Convert.ToDouble(order.QuantityInRunningMeter / order.FilmRecipe.ProductionSpeed);
    }
}

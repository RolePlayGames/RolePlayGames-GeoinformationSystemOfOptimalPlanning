using GSOP.Domain.Contracts.Orders;

namespace GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Time;

public interface IOrderExcecutionTimeCalculator
{
    /// <summary>
    /// Calculate order execution time, min
    /// </summary>
    /// <param name="order">Order to calculate</param>
    /// <returns>Order calculation time, min</returns>
    double Calculate(IOrder order);
}

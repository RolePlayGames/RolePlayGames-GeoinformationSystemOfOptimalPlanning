using GSOP.Domain.Contracts.Orders;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Cost.Base;

public interface IOrderCostCalculator
{
    double Calculate(IOrder order);
}

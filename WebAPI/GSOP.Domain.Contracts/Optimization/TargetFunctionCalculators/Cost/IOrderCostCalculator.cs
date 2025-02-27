using GSOP.Domain.Contracts.Orders;

namespace GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Cost;

public interface IOrderCostCalculator
{
    double Calculate(IOrder order);
}

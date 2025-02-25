using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Optimization.TargetFunctionCalculators.Cost.Base;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Cost;

public class OrderCostCalculator : IOrderCostCalculator
{
    public double Calculate(IOrder order) => (order.Waste + order.FinishedGoods) * order.FilmRecipe.MaterialCost;
}

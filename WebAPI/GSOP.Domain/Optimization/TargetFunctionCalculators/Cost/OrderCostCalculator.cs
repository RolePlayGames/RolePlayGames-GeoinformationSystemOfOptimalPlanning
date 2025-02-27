using GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Cost;
using GSOP.Domain.Contracts.Orders;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Cost;

public class OrderCostCalculator : IOrderCostCalculator
{
    public double Calculate(IOrder order) => (order.Waste + order.FinishedGoods) * order.FilmRecipe.MaterialCost;
}

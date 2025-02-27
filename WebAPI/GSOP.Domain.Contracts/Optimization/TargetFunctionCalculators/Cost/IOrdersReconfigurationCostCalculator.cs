using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;

namespace GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Cost;

public interface IOrdersReconfigurationCostCalculator
{
    double Calculate(IProductionLine productionLine, IOrder orderFrom, IOrder orderTo);
}

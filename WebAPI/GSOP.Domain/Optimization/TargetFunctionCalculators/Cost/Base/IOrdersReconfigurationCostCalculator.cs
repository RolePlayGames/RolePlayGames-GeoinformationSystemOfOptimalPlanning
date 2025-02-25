using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Cost.Base;

public interface IOrdersReconfigurationCostCalculator
{
    double Calculate(IProductionLine productionLine, IOrder orderFrom, IOrder orderTo);
}

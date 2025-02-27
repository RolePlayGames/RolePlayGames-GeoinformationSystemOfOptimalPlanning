using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;

namespace GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Time;

public interface IOrdersReconfigurationTimeCalculator
{
    double Calculate(IProductionLine productionLine, IOrder orderFrom, IOrder orderTo);
}

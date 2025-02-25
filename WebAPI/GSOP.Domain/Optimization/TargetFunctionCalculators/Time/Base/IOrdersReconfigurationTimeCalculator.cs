using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Time.Base;

public interface IOrdersReconfigurationTimeCalculator
{
    double Calculate(IProductionLine productionLine, IOrder orderFrom, IOrder orderTo);
}

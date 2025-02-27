using GSOP.Domain.Contracts.Optimization.Models;

namespace GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Cost;

public interface IProductionLineQueueCostCalculator
{
    double Calculate(ProductionLineQueue productionLineQueue);
}

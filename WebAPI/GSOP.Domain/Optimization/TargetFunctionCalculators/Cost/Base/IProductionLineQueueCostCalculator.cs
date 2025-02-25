using GSOP.Domain.Contracts.Optimization.Genetic.Models;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Cost.Base;

public interface IProductionLineQueueCostCalculator
{
    double Calculate(ProductionLineQueue productionLineQueue);
}

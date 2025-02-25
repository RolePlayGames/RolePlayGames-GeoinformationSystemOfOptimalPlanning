using GSOP.Domain.Contracts.Optimization.Genetic.Models;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Time.Base;

public interface IProductionLineQueueTimeCalculator
{
    double Calculate(ProductionLineQueue productionLineQueue);
}

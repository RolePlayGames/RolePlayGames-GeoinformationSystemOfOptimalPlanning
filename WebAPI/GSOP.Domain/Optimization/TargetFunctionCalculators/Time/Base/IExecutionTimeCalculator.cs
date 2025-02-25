using GSOP.Domain.Contracts.Optimization.Genetic.Models;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Time.Base;

public interface IExecutionTimeCalculator
{
    double Calculate(ProductionLineQueue productionLineQueue);
}

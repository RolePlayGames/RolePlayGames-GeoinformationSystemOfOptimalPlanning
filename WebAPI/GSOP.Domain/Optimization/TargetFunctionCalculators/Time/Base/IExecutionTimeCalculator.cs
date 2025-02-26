using GSOP.Domain.Contracts.Optimization.Models;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Time.Base;

public interface IExecutionTimeCalculator
{
    double Calculate(ProductionLineQueue productionLineQueue);
}

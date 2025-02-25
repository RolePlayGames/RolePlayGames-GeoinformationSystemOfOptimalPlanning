using GSOP.Domain.Contracts.Optimization.Genetic.Models;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Time.Base;

public interface IReconfigurationTimeCalculator
{
    double Calculate(ProductionLineQueue productionLineQueue);
}

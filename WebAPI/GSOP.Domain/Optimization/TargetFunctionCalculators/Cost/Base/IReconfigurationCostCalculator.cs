using GSOP.Domain.Contracts.Optimization.Genetic.Models;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Cost.Base;

public interface IReconfigurationCostCalculator
{
    double Calculate(ProductionLineQueue productionLineQueue);
}

using GSOP.Domain.Contracts.Optimization.Models;

namespace GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Cost;

public interface IReconfigurationCostCalculator
{
    double Calculate(ProductionLineQueue productionLineQueue);
}

using GSOP.Domain.Contracts.Optimization.Models;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Cost.Base;

public interface IReconfigurationCostCalculator
{
    double Calculate(ProductionLineQueue productionLineQueue);
}

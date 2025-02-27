using GSOP.Domain.Contracts.Optimization.Models;

namespace GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Time;

public interface IProductionLineQueueTimeCalculator
{
    double Calculate(ProductionLineQueue productionLineQueue);
}

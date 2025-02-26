using GSOP.Domain.Contracts.Optimization.Models;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Time.Base;

public interface IProductionLineQueueTimeCalculator
{
    double Calculate(ProductionLineQueue productionLineQueue);
}

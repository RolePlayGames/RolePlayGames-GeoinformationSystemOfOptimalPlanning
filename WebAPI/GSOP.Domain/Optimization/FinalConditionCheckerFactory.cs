using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Contracts.Optimization;
using GSOP.Domain.Contracts.Optimization.Models;
using GSOP.Domain.Optimization.FinalConditionChecker;

namespace GSOP.Domain.Optimization;

public class FinalConditionCheckerFactory : IFinalConditionCheckerFactory
{
    public IReadOnlyCollection<IFinalConditionChecker<ProductionPlan>> CreateFinalConditionCheckers(FinalCheckerConditions conditions)
    {
        var result = new List<IFinalConditionChecker<ProductionPlan>>();

        if (conditions.TimeoutDelay.HasValue)
        {
            result.Add(new TimeFinalConditionChecker<ProductionPlan>(conditions.TimeoutDelay.Value));
        }

        if (conditions.IterationsCount.HasValue)
        {
            result.Add(new IterationsFinalConditionChecker<ProductionPlan>(conditions.IterationsCount.Value));
        }

        return result;
    }
}

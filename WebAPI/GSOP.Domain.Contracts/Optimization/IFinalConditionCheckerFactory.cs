using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Contracts.Optimization.Models;

namespace GSOP.Domain.Contracts.Optimization;

public interface IFinalConditionCheckerFactory
{
    IReadOnlyCollection<IFinalConditionChecker<ProductionPlan>> CreateFinalConditionCheckers(FinalCheckerConditions conditions);
}

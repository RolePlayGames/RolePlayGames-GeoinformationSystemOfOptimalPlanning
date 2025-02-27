using GSOP.Domain.Contracts.Optimization.Models;

namespace GSOP.Domain.Contracts.Optimization;

public enum FunctionType { Time, Cost }

public interface IProductionPlanner
{
    Task<IReadOnlyCollection<ProductionPlanInfo>> CreateOptimizedProductionPlanByBruteforceAlgorithm(BruteforceAlgorithmPlanningData planningData);

    Task<IReadOnlyCollection<ProductionPlanInfo>> CreateOptimizedProductionPlanByGeneticAlgorithm(GeneticAlgorithmPlanningData planningData);
}

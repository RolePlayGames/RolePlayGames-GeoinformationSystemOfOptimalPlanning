using GSOP.Application.Contracts.Optimization.Models;
using GSOP.Domain.Contracts.Optimization.Models;

namespace GSOP.Domain.Contracts.Optimization;

public enum FunctionType { Time, Cost }

public interface IProductionPlanner
{
    Task<PlanningInfo> CreateOptimizedProductionPlanByBruteforceAlgorithm(BruteforceAlgorithmPlanningData planningData);

    Task<PlanningInfo> CreateOptimizedProductionPlanByGeneticAlgorithm(GeneticAlgorithmPlanningData planningData);

    Task<ProductionPlanInfo> GetOriginalProductionPlan(DateTime startDateTime);
}

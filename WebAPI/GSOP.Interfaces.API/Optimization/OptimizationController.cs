using GSOP.Application.Contracts.Optimization.Models;
using GSOP.Domain.Contracts.Optimization;
using GSOP.Domain.Contracts.Optimization.Models;
using Microsoft.AspNetCore.Mvc;

namespace GSOP.Interfaces.API.Optimization;

[ApiController]
[Route("api/optimization")]
public class OptimizationController
{
    private readonly ILogger<OptimizationController> _logger;
    private readonly IProductionPlanner _productionPlanner;

    public OptimizationController(ILogger<OptimizationController> logger, IProductionPlanner productionPlanner)
    {
        _logger = logger;
        _productionPlanner = productionPlanner;
    }

    [HttpPost]
    [Route("genetic")]
    public Task<PlanningInfo> OptimizeByGeneticAlgorithm(GeneticAlgorithmPlanningData planningData)
    {
        return _productionPlanner.CreateOptimizedProductionPlanByGeneticAlgorithm(planningData);
    }

    [HttpPost]
    [Route("bruteforce")]
    public Task<PlanningInfo> OptimizeByBruteforceAlgorithm(BruteforceAlgorithmPlanningData planningData)
    {
        return _productionPlanner.CreateOptimizedProductionPlanByBruteforceAlgorithm(planningData);
    }

    [HttpGet]
    [Route("original-plan")]
    public Task<ProductionPlanInfo> OriginalPlan(DateTime startDateTime)
    {
        return _productionPlanner.GetOriginalProductionPlan(startDateTime);
    }
}

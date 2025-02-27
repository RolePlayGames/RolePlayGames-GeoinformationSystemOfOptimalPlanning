using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Bruteforce;
using GSOP.Domain.Contracts.Optimization.Models;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;

namespace GSOP.Domain.Contracts.Optimization.Bruteforce;

public interface IBruteforceAlgorithmFactory
{
    IOptimizationAlgorithmDecisions<ProductionPlan> CreateAlgorithm(
        IBruteforceDistributor _bruteforeDistributor,
        IReadOnlyCollection<IOrder> orders,
        IReadOnlyCollection<IProductionLine> productionLines,
        FinalCheckerConditions conditions,
        IFitnessCalculator<ProductionPlan> fitnessCalculator);
}

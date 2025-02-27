using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Bruteforce;
using GSOP.Domain.Contracts.Optimization;
using GSOP.Domain.Contracts.Optimization.Bruteforce;
using GSOP.Domain.Contracts.Optimization.Models;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using OPTEL.Optimization.Algorithms.Bruteforce;

namespace GSOP.Domain.Optimization.Bruteforce;

public class BruteforceAlgorithmFactory : IBruteforceAlgorithmFactory
{
    private readonly IFinalConditionCheckerFactory _finalConditionCheckerFactory;

    public BruteforceAlgorithmFactory(IFinalConditionCheckerFactory finalConditionCheckerFactory)
    {
        _finalConditionCheckerFactory = finalConditionCheckerFactory;
    }

    public IOptimizationAlgorithmDecisions<ProductionPlan> CreateAlgorithm(
        IBruteforceDistributor bruteforeDistributor,
        IReadOnlyCollection<IOrder> orders,
        IReadOnlyCollection<IProductionLine> productionLines,
        FinalCheckerConditions conditions,
        IFitnessCalculator<ProductionPlan> fitnessCalculator)
    {
        var finalConditionCheckers = _finalConditionCheckerFactory.CreateFinalConditionCheckers(conditions);
        return new BruteforceAlgorithm(bruteforeDistributor, orders, productionLines, finalConditionCheckers, fitnessCalculator);
    }
}

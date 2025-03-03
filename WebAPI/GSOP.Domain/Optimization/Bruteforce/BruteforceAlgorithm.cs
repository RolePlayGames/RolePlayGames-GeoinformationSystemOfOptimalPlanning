using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Bruteforce;
using GSOP.Domain.Algorithms.Genetic.Extensions;
using GSOP.Domain.Contracts.Optimization.Models;

namespace OPTEL.Optimization.Algorithms.Bruteforce;

public class BruteforceAlgorithm : IOptimizationAlgorithm<ProductionPlan>, IOptimizationAlgorithmDecisions<ProductionPlan>
{
    private readonly IBruteforceDistributor _bruteforeDistributor;
    private readonly IReadOnlyCollection<IOrder> _orders;
    private readonly IReadOnlyCollection<IProductionLine> _productionLines;
    private readonly IReadOnlyCollection<IFinalConditionChecker<ProductionPlan>> _finalConditionCheckers;
    private readonly IFitnessCalculator<ProductionPlan> _fitnessCalculator;

    public BruteforceAlgorithm(
        IBruteforceDistributor bruteforeDistributor,
        IReadOnlyCollection<IOrder> orders,
        IReadOnlyCollection<IProductionLine> productionLines,
        IReadOnlyCollection<IFinalConditionChecker<ProductionPlan>> finalConditionCheckers,
        IFitnessCalculator<ProductionPlan> fitnessCalculator)
    {
        _bruteforeDistributor = bruteforeDistributor;
        _orders = orders;
        _productionLines = productionLines;
        _finalConditionCheckers = finalConditionCheckers;
        _fitnessCalculator = fitnessCalculator;
    }

    ProductionPlan IOptimizationAlgorithm<ProductionPlan>.GetResolve()
    {
        return ((IOptimizationAlgorithmDecisions<ProductionPlan>)this).GetResolve().LastOrDefault();
    }

    public IEnumerable<ProductionPlan> GetResolve()
    {
        var bestFintess = double.MinValue;
        ProductionPlan? bestResolve = null;

        foreach (var resolve in GetResolvesInternal())
        {
            var fitness = _fitnessCalculator.Calculate(resolve);

            if (bestResolve is null || bestFintess < fitness)
            {
                bestResolve = resolve;
                bestFintess = fitness;
                yield return resolve;
            }
        }
    }

    private IEnumerable<ProductionPlan> GetResolvesInternal()
    {
        _finalConditionCheckers.ForEach(x => x.Begin());

        foreach (var decision in _bruteforeDistributor.DistributeAllItemsBetweenAllBuckets(_productionLines, _orders))
        {
            var resolve = ConvertDecision(decision);

            yield return resolve;

            if (_finalConditionCheckers.Any(x => x.IsStateFinal(resolve)))
                yield break;
        }
    }

    private ProductionPlan ConvertDecision(List<List<IOrder>> combinations)
    {
        var queues = new List<ProductionLineQueue>(combinations.Count);

        for (var i = 0; i < combinations.Count; i++)
        {
            queues.Add(new() { ProductionLine = _productionLines.ElementAt(i), Orders = combinations[i] });
        }

        return new() { ProductionLineQueues = queues };
    }
}

using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Bruteforce;
using GSOP.Domain.Algorithms.Genetic.Extensions;

namespace OPTEL.Optimization.Algorithms.Bruteforce;

public class BruteforceAlgorithm : IOptimizationAlgorithm<IReadOnlyDictionary<IProductionLine, List<IOrder>>>, IOptimizationAlgorithmDecisions<IReadOnlyDictionary<IProductionLine, List<IOrder>>>
{
    private readonly IBruteforeDistributor _bruteforeDistributor;
    private readonly IReadOnlyCollection<IOrder> _orders;
    private readonly IReadOnlyCollection<IProductionLine> _productionLines;
    private readonly IReadOnlyCollection<IFinalConditionChecker<IReadOnlyDictionary<IProductionLine, List<IOrder>>>> _finalConditionCheckers;
    private readonly IFitnessCalculator<IReadOnlyDictionary<IProductionLine, List<IOrder>>> _fitnessCalculator;

    public BruteforceAlgorithm(
        IBruteforeDistributor bruteforeDistributor,
        IReadOnlyCollection<IOrder> orders,
        IReadOnlyCollection<IProductionLine> productionLines,
        IReadOnlyCollection<IFinalConditionChecker<IReadOnlyDictionary<IProductionLine, List<IOrder>>>> finalConditionCheckers,
        IFitnessCalculator<IReadOnlyDictionary<IProductionLine, List<IOrder>>> fitnessCalculator)
    {
        _bruteforeDistributor = bruteforeDistributor;
        _orders = orders;
        _productionLines = productionLines;
        _finalConditionCheckers = finalConditionCheckers;
        _fitnessCalculator = fitnessCalculator;
    }

    IReadOnlyDictionary<IProductionLine, List<IOrder>> IOptimizationAlgorithm<IReadOnlyDictionary<IProductionLine, List<IOrder>>>.GetResolve()
    {
        return ((IOptimizationAlgorithmDecisions<IReadOnlyDictionary<IProductionLine, List<IOrder>>>)this).GetResolve().LastOrDefault();
    }

    public IEnumerable<IReadOnlyDictionary<IProductionLine, List<IOrder>>> GetResolve()
    {
        var bestFintess = double.MinValue;
        IReadOnlyDictionary<IProductionLine, List<IOrder>>? bestResolve = null;

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

    private IEnumerable<IReadOnlyDictionary<IProductionLine, List<IOrder>>> GetResolvesInternal()
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

    private IReadOnlyDictionary<IProductionLine, List<IOrder>> ConvertDecision(List<List<int>> combinations)
    {
        var result = new Dictionary<IProductionLine, List<IOrder>>(combinations.Count);

        for (var i = 0; i < combinations.Count; i++)
        {
            result[_productionLines.ElementAt(i)] = combinations[i].Select(x => _orders.ElementAt(x)).ToList();
        }

        return result;
    }
}

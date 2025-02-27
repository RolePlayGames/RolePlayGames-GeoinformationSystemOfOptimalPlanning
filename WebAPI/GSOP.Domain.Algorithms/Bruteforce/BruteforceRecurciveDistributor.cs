using GSOP.Domain.Algorithms.Bruteforce.CombinationsGenerators;
using GSOP.Domain.Algorithms.Contracts.Bruteforce;

namespace GSOP.Domain.Algorithms.Bruteforce;

public class BruteforceRecurciveDistributor : IBruteforceDistributor
{
    private readonly ICombinationsGenerator _combinationsGenerator;

    public BruteforceRecurciveDistributor(ICombinationsGenerator combinationsGenerator)
    {
        _combinationsGenerator = combinationsGenerator;
    }

    public IEnumerable<List<List<int>>> DistributeAllItemsBetweenAllBuckets<TItem, TBucket>(IReadOnlyCollection<TBucket> buckets, IReadOnlyCollection<TItem> items)
    {
        foreach (var lineDistributions in GenerateAllDistributions(items.Count, buckets.Count))
        {
            foreach (var distribution in GetAllDistributions(lineDistributions, 0, lineDistributions.Select(x => new List<int>(x.Count)).ToList()))
            {
                yield return distribution;
            }
        }
    }

    private IEnumerable<List<List<int>>> GetAllDistributions(List<List<int>> originalDistributions, int index, List<List<int>> currentDistributions)
    {
        if (originalDistributions.Count <= index)
        {
            yield return currentDistributions;
        }
        else
        {
            foreach (var distributionCombination in _combinationsGenerator.GenerateCombinations(originalDistributions[index]))
            {
                currentDistributions[index] = distributionCombination;

                foreach (var distribution in GetAllDistributions(originalDistributions, index + 1, currentDistributions))
                {
                    yield return distribution;
                }
            }
        }
    }

    public IEnumerable<List<List<int>>> GenerateAllDistributions(int ordersCount, int productionLinesCount)
    {
        var initialDistribution = Enumerable.Range(0, productionLinesCount).Select(_ => new List<int>()).ToList();

        return Distribute(0, ordersCount, initialDistribution);
    }

    private IEnumerable<List<List<int>>> Distribute(int index, int ordersCount, List<List<int>> currentDistribution)
    {
        if (ordersCount <= index)
        {
            yield return currentDistribution;
        }
        else
        {
            for (var i = 0; i < currentDistribution.Count; i++)
            {
                var newDistribution = currentDistribution.Select(basket => new List<int>(basket)).ToList();

                newDistribution[i].Add(index);

                foreach (var distribution in Distribute(index + 1, ordersCount, newDistribution))
                {
                    yield return distribution;
                }
            }
        }
    }
}

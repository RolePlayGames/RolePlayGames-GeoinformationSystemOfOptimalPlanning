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

    public IEnumerable<List<List<TItem>>> DistributeAllItemsBetweenAllBuckets<TItem, TBucket>(IReadOnlyCollection<TBucket> buckets, IReadOnlyCollection<TItem> items)
    {
        foreach (var lineDistributions in GenerateAllDistributions(items.Count, buckets.Count))
        {
            foreach (var distribution in GetAllDistributions(items.ToList(), lineDistributions, 0, Enumerable.Range(0, buckets.Count).Select(_ => new List<TItem>()).ToList()))
            {
                yield return distribution;
            }
        }
    }

    private IEnumerable<List<List<TItem>>> GetAllDistributions<TItem>(List<TItem> items, List<List<int>> originalDistributions, int index, List<List<TItem>> currentDistributions)
    {
        if (originalDistributions.Count <= index)
        {
            yield return currentDistributions;
        }
        else
        {
            var bucketItems = new List<TItem>();
            foreach (var itemIndex in originalDistributions[index])
            {
                bucketItems.Add(items[itemIndex]);
            }

            foreach (var permutation in _combinationsGenerator.GenerateCombinations(bucketItems.Select((item, i) => i).ToList()))
            {
                var permutedBucket = new List<TItem>();
                foreach (var itemIndex in permutation)
                {
                    permutedBucket.Add(bucketItems[itemIndex]);
                }

                var nextCurrentDistributions = currentDistributions.Select(x => x.ToList()).ToList();
                nextCurrentDistributions[index] = permutedBucket;

                foreach (var distribution in GetAllDistributions(items, originalDistributions, index + 1, nextCurrentDistributions))
                {
                    yield return distribution;
                }
            }
        }
    }

    public static IEnumerable<List<List<int>>> GenerateAllDistributions(int ordersCount, int productionLinesCount)
    {
        var initialDistribution = Enumerable.Range(0, productionLinesCount).Select(_ => new List<int>()).ToList();

        return Distribute(0, ordersCount, initialDistribution);
    }

    private static IEnumerable<List<List<int>>> Distribute(int index, int ordersCount, List<List<int>> currentDistribution)
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

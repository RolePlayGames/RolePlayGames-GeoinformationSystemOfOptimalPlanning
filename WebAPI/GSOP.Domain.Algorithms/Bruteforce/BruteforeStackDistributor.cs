using GSOP.Domain.Algorithms.Bruteforce.CombinationsGenerators;
using GSOP.Domain.Algorithms.Contracts.Bruteforce;

namespace GSOP.Domain.Algorithms.Bruteforce;

public class BruteforeStackDistributor : IBruteforceDistributor
{
    private readonly ICombinationsGenerator _combinationsGenerator;

    public BruteforeStackDistributor(ICombinationsGenerator combinationsGenerator)
    {
        _combinationsGenerator = combinationsGenerator;
    }

    public IEnumerable<List<List<TItem>>> DistributeAllItemsBetweenAllBuckets<TItem, TBucket>(IReadOnlyCollection<TBucket> buckets, IReadOnlyCollection<TItem> items)
    {
        foreach (var bucketDistributions in GenerateAllDistributionsIterative(items.Count, buckets.Count))
        {
            foreach (var distribution in GetAllPermutationsIterative([.. items], bucketDistributions))
            {
                yield return distribution;
            }
        }
    }

    private static IEnumerable<List<List<int>>> GenerateAllDistributionsIterative(int ordersCount, int productionLinesCount)
    {
        Stack<(int index, int line, List<List<int>> currentDistribution)> stack = new();
        stack.Push((0, 0, Enumerable.Range(0, productionLinesCount).Select(_ => new List<int>()).ToList()));

        while (stack.Count > 0)
        {
            (var index, var line, var currentDistribution) = stack.Pop();

            if (index == ordersCount)
            {
                yield return currentDistribution;
            }
            else
            {
                for (var i = line; i < productionLinesCount; i++)
                {
                    var newDistribution = currentDistribution.Select(x => x.ToList()).ToList();

                    newDistribution[i].Add(index);

                    if (i == productionLinesCount - 1)
                    {
                        stack.Push((index + 1, 0, newDistribution));
                    }
                    else
                    {
                        stack.Push((index, i + 1, newDistribution));
                    }
                }
            }
        }
    }

    private IEnumerable<List<List<TItem>>> GetAllPermutationsIterative<TItem>(List<TItem> items, List<List<int>> originalDistributions)
    {
        var numBuckets = originalDistributions.Count;

        Stack<(int index, List<List<TItem>> currentDistributions, IEnumerator<List<int>>? enumerator)> stack = new();

        stack.Push((0, Enumerable.Range(0, numBuckets).Select(_ => new List<TItem>()).ToList(), (originalDistributions.Count > 0 && originalDistributions[0].Count > 0) ? _combinationsGenerator.GenerateCombinations(originalDistributions[0]).GetEnumerator() : null));

        while (stack.Count > 0)
        {
            (var index, var currentDistributions, var enumerator) = stack.Peek();

            if (enumerator is null || index == numBuckets)
            {
                yield return currentDistributions;
                stack.Pop();
            }
            else if (enumerator.MoveNext())
            {
                var newDistributions = currentDistributions.Select(x => x.ToList()).ToList();

                for (var i = 0; i < numBuckets; i++)
                {
                    newDistributions[i].Clear();

                    foreach (var itemIndex in originalDistributions[i])
                    {
                        newDistributions[i].Add(items[itemIndex]);
                    }
                }

                var nextIndex = index + 1;

                if (nextIndex < numBuckets)
                {
                    stack.Push((nextIndex, newDistributions, (originalDistributions.Count > nextIndex && originalDistributions[nextIndex].Count > 0) ? _combinationsGenerator.GenerateCombinations(originalDistributions[nextIndex]).GetEnumerator() : null));
                }
                else
                {
                    stack.Push((nextIndex, newDistributions, null));
                }
            }
            else
            {
                enumerator.Dispose();
                stack.Pop();
            }
        }
    }
}

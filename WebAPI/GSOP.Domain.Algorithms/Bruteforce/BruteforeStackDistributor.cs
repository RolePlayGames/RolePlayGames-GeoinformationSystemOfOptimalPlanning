using GSOP.Domain.Algorithms.Bruteforce.CombinationsGenerators;
using GSOP.Domain.Algorithms.Contracts.Bruteforce;

namespace GSOP.Domain.Algorithms.Bruteforce;

public class BruteforeStackDistributor : IBruteforeDistributor
{
    private readonly ICombinationsGenerator _combinationsGenerator;

    public BruteforeStackDistributor(ICombinationsGenerator combinationsGenerator)
    {
        _combinationsGenerator = combinationsGenerator;
    }

    public IEnumerable<List<List<int>>> DistributeAllItemsBetweenAllBuckets<TItem, TBucket>(IReadOnlyCollection<TBucket> buckets, IReadOnlyCollection<TItem> items)
    {
        foreach (var lineDistributions in GenerateAllDistributionsIterative(items.Count, buckets.Count))
        {
            foreach (var distribution in GetAllPermutationsIterative(lineDistributions))
            {
                yield return distribution;
            }
        }
    }

    private IEnumerable<List<List<int>>> GenerateAllDistributionsIterative(int ordersCount, int productionLinesCount)
    {
        Stack<(int index, int line, List<List<int>> currentDistribution)> stack = new Stack<(int, int, List<List<int>>)>();
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

                    newDistribution[i].Add(0);

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

    private IEnumerable<List<List<int>>> GetAllPermutationsIterative(List<List<int>> originalDistributions)
    {
        var numLines = originalDistributions.Count;

        Stack<(int index, List<List<int>> currentDistributions, IEnumerator<List<int>>? enumerator)> stack = new Stack<(int, List<List<int>>, IEnumerator<List<int>>?)>();

        stack.Push((0, originalDistributions.Select(x => x.ToList()).ToList(), _combinationsGenerator.GenerateCombinations(originalDistributions[0]).GetEnumerator()));

        while (stack.Count > 0)
        {
            (var index, var currentDistributions, var enumerator) = stack.Peek();

            if (enumerator is null || index == numLines)
            {
                yield return currentDistributions;
                stack.Pop();
            }
            else if (enumerator.MoveNext())
            {
                var permutation = enumerator.Current;

                var newDistributions = currentDistributions.Select(x => x.ToList()).ToList();
                newDistributions[index] = permutation;

                if (index + 1 < numLines)
                {
                    stack.Push((index + 1, newDistributions, _combinationsGenerator.GenerateCombinations(originalDistributions[index + 1]).GetEnumerator()));
                }
                else
                {
                    stack.Push((index + 1, newDistributions, null));
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

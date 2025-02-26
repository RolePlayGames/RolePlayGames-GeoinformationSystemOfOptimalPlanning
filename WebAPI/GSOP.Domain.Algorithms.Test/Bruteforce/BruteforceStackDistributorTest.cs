using FluentAssertions;
using GSOP.Domain.Algorithms.Bruteforce;
using GSOP.Domain.Algorithms.Bruteforce.CombinationsGenerators;

namespace GSOP.Domain.Algorithms.Test.Bruteforce;

public class BruteforceStackDistributorTest
{
    private readonly CombinationsGenerator _combinationsGenerator;
    private readonly BruteforeStackDistributor _bruteforeStackDistributor;

    public BruteforceStackDistributorTest()
    {
        _combinationsGenerator = new();
        _bruteforeStackDistributor = new(_combinationsGenerator);
    }

    [Fact]
    public void DistributeAllItemsBetweenAllBuckets_With1BucketAnd3Items_ReturnsUniqueDistributionFor1Bcuket()
    {
        // Arrange
        var expectedCount = 6;
        var buckets = new object[1];
        var items = new object[3];
        var result = new List<List<List<int>>>(expectedCount);

        // Act & Assert
        foreach (var distribution in _bruteforeStackDistributor.DistributeAllItemsBetweenAllBuckets(buckets, items))
        {
            var materializeDistribution = distribution.ToList();
            materializeDistribution.Should().HaveCount(buckets.Length);

            foreach (var combination in materializeDistribution)
            {
                combination.Should().OnlyHaveUniqueItems();

                foreach (var differCombination in materializeDistribution.Where(x => x != combination))
                {
                    combination.Should().NotContainInOrder(differCombination);
                }

            }
        }
    }
}

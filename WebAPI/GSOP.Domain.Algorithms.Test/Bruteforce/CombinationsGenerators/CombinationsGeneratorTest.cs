using FluentAssertions;
using GSOP.Domain.Algorithms.Bruteforce.CombinationsGenerators;

namespace GSOP.Domain.Algorithms.Test.Bruteforce.CombinationsGenerators;

public class CombinationsGeneratorTest
{
    private readonly CombinationsGenerator _combinationsGenerator;

    public CombinationsGeneratorTest()
    {
        _combinationsGenerator = new();
    }

    [Fact]
    public void GenerateCombinations_For3Numbers_Returns6ResultsWith()
    {
        // Arrange
        var expectedCount = 6;
        var variations = new List<int> { 1, 2, 3 };

        // Act
        var result = _combinationsGenerator.GenerateCombinations(variations).ToList();

        // Assert
        result.Should().HaveCount(expectedCount);

        foreach (var combination in result)
        {
            combination.Should().OnlyHaveUniqueItems();

            foreach (var differCombination in result.Where(x => x != combination))
            {
                combination.Should().NotContainInOrder(differCombination);
            }
        }
    }
}

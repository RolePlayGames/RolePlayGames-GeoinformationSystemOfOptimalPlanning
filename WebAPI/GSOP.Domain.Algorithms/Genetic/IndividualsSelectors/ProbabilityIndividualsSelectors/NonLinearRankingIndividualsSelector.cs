using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Algorithms.Genetic.Probabilities;

namespace GSOP.Domain.Algorithms.Genetic.IndividualsSelectors.ProbabilityIndividualsSelectors;

public class NonLinearRankingIndividualsSelector<TGene> : ProbabilityIndividualsSelector<TGene> where TGene : IGene
{
    public const double COEFFICIENT_MIN_VALUE = 0;
    public const double COEFFICIENT_MAX_VALUE = 1;

    protected double Coefficient { get; }

    public NonLinearRankingIndividualsSelector(double coefficient, RandomProbabilityChecker randomProbabilityChecker, int selectionCount)
        : base(randomProbabilityChecker, selectionCount)
    {
        if (coefficient < COEFFICIENT_MIN_VALUE || coefficient > COEFFICIENT_MAX_VALUE)
            throw new ArgumentOutOfRangeException(nameof(coefficient), $"Coefficient should be between {COEFFICIENT_MIN_VALUE} and {COEFFICIENT_MAX_VALUE}");

        Coefficient = coefficient;
    }

    protected override IEnumerable<ItemProbability<IIndividual<TGene>>> CalculateProbabilities(IReadOnlyCollection<IIndividual<TGene>> individuals)
    {
        var sortedArray = individuals.OrderBy(x => x.FitnessFunctionValue).ToArray();

        var populationCount = sortedArray.Length;

        for (var i = 0; i < sortedArray.Length; i++)
        {
            var probability = Coefficient * Math.Pow(1 - Coefficient, populationCount - (i + 1));

            yield return new ItemProbability<IIndividual<TGene>>(sortedArray[i], probability);
        }
    }
}

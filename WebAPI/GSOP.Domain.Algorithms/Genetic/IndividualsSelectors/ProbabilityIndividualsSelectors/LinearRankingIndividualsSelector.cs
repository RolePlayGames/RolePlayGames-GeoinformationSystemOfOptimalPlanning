using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Algorithms.Genetic.Probabilities;

namespace GSOP.Domain.Algorithms.Genetic.IndividualsSelectors.ProbabilityIndividualsSelectors;

public class LinearRankingIndividualsSelector<TGene> : ProbabilityIndividualsSelector<TGene> where TGene : IGene
{
    public const double COEFFICIENT_MIN_VALUE = 1;
    public const double COEFFICIENT_MAX_VALUE = 2;

    protected double Coefficient { get; }

    public LinearRankingIndividualsSelector(double coefficient, RandomProbabilityChecker randomProbabilityChecker, int selectionCount)
        : base(randomProbabilityChecker, selectionCount)
    {
        if (coefficient < COEFFICIENT_MIN_VALUE || coefficient > COEFFICIENT_MAX_VALUE)
            throw new ArgumentOutOfRangeException(nameof(coefficient), $"Coefficient should be between {COEFFICIENT_MIN_VALUE} and {COEFFICIENT_MAX_VALUE}");

        Coefficient = coefficient;
    }

    protected override IEnumerable<ItemProbability<IIndividual<TGene>>> CalculateProbabilities(IReadOnlyCollection<IIndividual<TGene>> individuals)
    {
        var sortedArray = individuals.OrderByDescending(x => x.FitnessFunctionValue).ToList();

        var populationCount = (double)sortedArray.Count;

        for (var i = 0; i < sortedArray.Count; i++)
        {
            var probability = 1 / populationCount * (Coefficient - (2 * Coefficient - 2) * (i / (populationCount - 1)));
            
            yield return new ItemProbability<IIndividual<TGene>>(sortedArray[i], probability);
        }
    }
}

using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Algorithms.Genetic.Probabilities;

namespace GSOP.Domain.Algorithms.Genetic.IndividualsSelectors.ProbabilityIndividualsSelectors;

public class RouletteIndividualsSelector<TGene> : ProbabilityIndividualsSelector<TGene> where TGene : IGene
{
    public RouletteIndividualsSelector(RandomProbabilityChecker randomProbabilityChecker, int selectionCount)
        : base(randomProbabilityChecker, selectionCount)
    {

    }

    protected override IEnumerable<ItemProbability<IIndividual<TGene>>> CalculateProbabilities(IReadOnlyCollection<IIndividual<TGene>> individuals)
    {
        var sumFitness = individuals.Sum(x => x.FitnessFunctionValue);

        foreach (var individual in individuals)
        {
            var probability = individual.FitnessFunctionValue / sumFitness;

            yield return new ItemProbability<IIndividual<TGene>>(individual, probability);
        }
    }
}

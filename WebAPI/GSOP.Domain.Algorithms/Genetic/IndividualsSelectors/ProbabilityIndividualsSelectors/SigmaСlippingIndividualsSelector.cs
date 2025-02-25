using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Algorithms.Genetic.Probabilities;

namespace GSOP.Domain.Algorithms.Genetic.IndividualsSelectors.ProbabilityIndividualsSelectors;

public class SigmaСlippingIndividualsSelector<TGene> : ProbabilityIndividualsSelector<TGene> where TGene : IGene
{
    public SigmaСlippingIndividualsSelector(RandomProbabilityChecker randomProbabilityChecker, int selectionCount) : base(randomProbabilityChecker, selectionCount)
    {

    }

    protected override IEnumerable<ItemProbability<IIndividual<TGene>>> CalculateProbabilities(IReadOnlyCollection<IIndividual<TGene>> individuals)
    {
        var averageTargetFunctionValue = individuals.Average(x => x.TargetFunctionValue);
        var dispertion = CalculateDispersion(individuals, averageTargetFunctionValue);

        var sumScalingObjectiveFunctionValue = individuals.Sum(x => CalculateScalingObjectiveFunctionValue(x, dispertion, averageTargetFunctionValue));

        foreach (var individual in individuals)
        {
            var probability = CalculateScalingObjectiveFunctionValue(individual, dispertion, averageTargetFunctionValue) / sumScalingObjectiveFunctionValue;

            yield return new ItemProbability<IIndividual<TGene>>(individual, probability);
        }
    }

    protected double CalculateDispersion(IEnumerable<IIndividual<TGene>> individuals, double averageTargetFunctionValue)
    {
        var sumDifferencesWithMean = individuals.Sum(x => Math.Pow(x.TargetFunctionValue - averageTargetFunctionValue, 2));

        return sumDifferencesWithMean / individuals.Count();
    }

    protected double CalculateScalingObjectiveFunctionValue(IIndividual<TGene> individual, double dispertion, double averageTargetFunctionValue)
    {
        return 1 + (individual.TargetFunctionValue - averageTargetFunctionValue) / (2 * Math.Sqrt(dispertion));
    }
}

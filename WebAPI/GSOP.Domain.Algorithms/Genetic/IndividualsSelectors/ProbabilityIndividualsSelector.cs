using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Algorithms.Genetic.Probabilities;

namespace GSOP.Domain.Algorithms.Genetic.IndividualsSelectors;

public abstract class ProbabilityIndividualsSelector<TGene> : IndividualsSelector<TGene> where TGene : IGene
{
    protected RandomProbabilityChecker RandomProbabilityChecker { get; }

    protected int SelectionCount { get; }

    public ProbabilityIndividualsSelector(RandomProbabilityChecker randomProbabilityChecker, int selectionCount)
    {
        if (selectionCount < 1)
            throw new ArgumentOutOfRangeException(nameof(selectionCount), "Selection count should be greater than 0");

        RandomProbabilityChecker = randomProbabilityChecker ?? throw new ArgumentNullException(nameof(randomProbabilityChecker), "Random probability checker shouldn't be null");
        SelectionCount = selectionCount;
    }

    protected override IEnumerable<IIndividual<TGene>> SelectIndividualsInternal(IReadOnlyCollection<IIndividual<TGene>> individuals)
    {
        var collection = individuals.ToList();

        for (var i = 0; i < SelectionCount && collection.Count > 0; i++)
        {
            var individualProbabilities = CalculateProbabilities(collection);
            var item = RandomProbabilityChecker.GetRandomItem(individualProbabilities);

            yield return item;

            collection.Remove(item);
        }
    }

    /// <summary>
    /// Calculate probabilities for individuals
    /// </summary>
    /// <param name="individuals">Individuals</param>
    /// <returns>Individuals with probabilities</returns>
    protected abstract IEnumerable<ItemProbability<IIndividual<TGene>>> CalculateProbabilities(IReadOnlyCollection<IIndividual<TGene>> individuals);
}

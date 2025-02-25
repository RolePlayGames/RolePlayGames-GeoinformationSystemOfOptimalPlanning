using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Algorithms.Genetic.IndividualsSelectors;

public class UniformRankingIndividualsSelector<TGene> : IndividualsSelector<TGene> where TGene : IGene
{
    private readonly Random _random;
    private readonly int _selectionCount;
    private readonly int _maxSelectionCount;

    public UniformRankingIndividualsSelector(Random random, int selectionCount, int maxSelectionCount = 0)
    {
        if (selectionCount < 1)
            throw new ArgumentOutOfRangeException(nameof(selectionCount), "Selection count should be greater than 0");

        if (maxSelectionCount < 0)
            throw new ArgumentOutOfRangeException(nameof(maxSelectionCount), "Max selection count should be greater than or equal to 0");
        else if (maxSelectionCount != 0 && maxSelectionCount < selectionCount)
            throw new ArgumentOutOfRangeException(nameof(maxSelectionCount), "Max selection count should be greater than or equal to selection count");

        _random = random ?? throw new ArgumentNullException(nameof(random), "Random object shouldn't be null");
        _selectionCount = selectionCount;
        _maxSelectionCount = maxSelectionCount;
    }

    protected override IEnumerable<IIndividual<TGene>> SelectIndividualsInternal(IReadOnlyCollection<IIndividual<TGene>> individuals)
    {
        if (individuals.Count <= 0)
            throw new ArgumentException("Current population individuals count should be grater than 0", nameof(individuals));
        else if (_maxSelectionCount != 0 && _maxSelectionCount > individuals.Count)
            throw new ArgumentException("Current population individuals count should be lesser than or equal to max selection count, if it is different to 0", nameof(individuals));

        return SelectRandomIndividuals(individuals);
    }

    private IEnumerable<IIndividual<TGene>> SelectRandomIndividuals(IReadOnlyCollection<IIndividual<TGene>> individuals)
    {
        var collection = individuals.ToList();
        var selectionCount = _selectionCount > collection.Count ? collection.Count : _selectionCount;
        var maxSelectionCount = _maxSelectionCount == 0 || _maxSelectionCount > collection.Count ? collection.Count : _maxSelectionCount;

        for (var i = 0; i < selectionCount; i++)
        {
            var index = _random.Next(0, maxSelectionCount);
            var item = collection.ElementAt(index);

            yield return item;

            collection.Remove(item);
            maxSelectionCount--;
        }
    }
}

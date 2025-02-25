using GSOP.Domain.Algorithms.Contracts.Genetic;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Algorithms.Contracts.Genetic.Operators;

namespace GSOP.Domain.Algorithms.Genetic;

public class PopulationSelector<TGene> : IPopulationSelector<TGene> where TGene : IGene
{
    private readonly IIndividualsSelector<TGene> _individualsSelector;

    public PopulationSelector(IIndividualsSelector<TGene> individualsSelector)
    {
        _individualsSelector = individualsSelector ?? throw new ArgumentNullException(nameof(individualsSelector), "Individual selctor should not be null");
    }

    public IReadOnlyCollection<IIndividual<TGene>> Select(IReadOnlyCollection<IIndividual<TGene>> individuals)
    {
        return individuals.Count < 2
            ? throw new ArgumentOutOfRangeException("Current population individuals count should be grater than or equal to 2", nameof(individuals))
            : (IReadOnlyCollection<IIndividual<TGene>>)_individualsSelector.SelectIndividuals(individuals).ToList();
    }
}

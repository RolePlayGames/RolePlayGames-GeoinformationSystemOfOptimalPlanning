using GSOP.Domain.Algorithms.Contracts.Genetic;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Algorithms.Genetic.IndividualsSelectors;

public abstract class IndividualsSelector<TGene> : IIndividualsSelector<TGene> where TGene : IGene
{
    public IEnumerable<IIndividual<TGene>> SelectIndividuals(IReadOnlyCollection<IIndividual<TGene>> individuals)
    {
        return individuals.Count > 0
            ? SelectIndividualsInternal(individuals)
            : throw new ArgumentException("Current population individuals count should be grater than 0", nameof(individuals));
    }

    /// <summary>
    /// Select some individuals from collection
    /// </summary>
    /// <param name="individualsCollection">Collection of individuals to select</param>
    /// <returns>Selected individuals</returns>
    protected abstract IEnumerable<IIndividual<TGene>> SelectIndividualsInternal(IReadOnlyCollection<IIndividual<TGene>> individuals);
}

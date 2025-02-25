using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Algorithms.Contracts.Genetic;

public interface IIndividualsSelector<TGene> where TGene : IGene
{
    /// <summary>
    /// Select some individuals from collection
    /// </summary>
    /// <param name="individuals">Individuals to select</param>
    /// <returns>Selected individuals</returns>
    IEnumerable<IIndividual<TGene>> SelectIndividuals(IReadOnlyCollection<IIndividual<TGene>> individuals);
}

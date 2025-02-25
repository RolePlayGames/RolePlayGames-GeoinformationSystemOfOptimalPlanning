using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Algorithms.Contracts.Genetic.Operators;

public interface ICrossoverOperatorSelector<TGene> where TGene : IGene
{
    /// <summary>
    /// Select parents from population to crossover
    /// </summary>
    /// <param name="individuals">Individuals to select</param>
    /// <returns>Collection of parents</returns>
    IEnumerable<Parents<TGene>> SelectParents(IReadOnlyCollection<IIndividual<TGene>> individuals);
}
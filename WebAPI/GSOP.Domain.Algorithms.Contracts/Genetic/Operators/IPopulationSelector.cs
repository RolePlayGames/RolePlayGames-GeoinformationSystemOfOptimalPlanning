using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Algorithms.Contracts.Genetic.Operators;

public interface IPopulationSelector<TGene> where TGene : IGene
{
    /// <summary>
    /// Selects best individuals
    /// </summary>
    /// <param name="individuals">Individuals</param>
    /// <returns>Best individuals</returns>
    IReadOnlyCollection<IIndividual<TGene>> Select(IReadOnlyCollection<IIndividual<TGene>> individuals);
}

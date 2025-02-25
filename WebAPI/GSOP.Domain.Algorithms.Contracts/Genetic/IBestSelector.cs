using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Algorithms.Contracts.Genetic;

public interface IBestSelector<TGene> where TGene : IGene
{
    /// <summary>
    /// Return best individual form individuals collection
    /// </summary>
    /// <param name="individuals">Individuals collection </param>
    /// <returns>Best individual</returns>
    IIndividual<TGene> SelectBestIndividual(IEnumerable<IIndividual<TGene>> individuals);
}

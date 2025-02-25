using GSOP.Domain.Algorithms.Contracts.Genetic;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Algorithms.Genetic;

public class BestSelector<TGene> : IBestSelector<TGene> where TGene : IGene
{
    /// <summary>
    /// Return best individual form individuals collection
    /// </summary>
    /// <param name="individuals">Individuals collection </param>
    /// <returns>Best individual</returns>
    public IIndividual<TGene> SelectBestIndividual(IEnumerable<IIndividual<TGene>> individuals)
    {
        return individuals is null
            ? throw new ArgumentNullException(nameof(individuals), "Enumerable of Individual is null")
            : individuals.MaxBy(x => x.FitnessFunctionValue) ?? throw new InvalidOperationException("Individuals pull is empty");
    }
}

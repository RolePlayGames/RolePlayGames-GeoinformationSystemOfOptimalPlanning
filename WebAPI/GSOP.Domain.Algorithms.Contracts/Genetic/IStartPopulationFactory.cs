using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Algorithms.Contracts.Genetic;

public interface IStartPopulationFactory<TGene> where TGene : IGene
{
    /// <summary>
    /// Creates start population
    /// </summary>
    /// <returns>Start population</returns>
    IPopulation<TGene> CreateStartPopulation();
}

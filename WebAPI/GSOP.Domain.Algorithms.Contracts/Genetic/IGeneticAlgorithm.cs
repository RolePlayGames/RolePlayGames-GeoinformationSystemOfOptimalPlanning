using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Algorithms.Contracts.Genetic;

public interface IGeneticAlgorithm<TGene> : IOptimizationAlgorithm<IIndividual<TGene>> where TGene : IGene
{

}

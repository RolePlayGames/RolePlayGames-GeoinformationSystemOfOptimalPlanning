using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Algorithms.Contracts.Genetic;

public interface IGeneticAlgorithmsDecisions<TGene> : IOptimizationAlgorithmDecisions<IIndividual<TGene>> where TGene : IGene
{

}

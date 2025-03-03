namespace GSOP.Domain.Algorithms.Contracts.Genetic.Models;

public interface IPopulation<TGene> where TGene : IGene
{
    IIndividual<TGene>? Best { get; }

    IPopulation<TGene> Reproduction();

    IPopulation<TGene> Mutation();

    IPopulation<TGene> Selection();

    IPopulation<TGene> IncludeApproximation(IIndividual<TGene> appriximation);
}

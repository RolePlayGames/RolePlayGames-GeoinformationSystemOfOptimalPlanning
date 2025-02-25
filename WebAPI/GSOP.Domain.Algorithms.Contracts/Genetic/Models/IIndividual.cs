namespace GSOP.Domain.Algorithms.Contracts.Genetic.Models;

public interface IIndividual<TGene> where TGene : IGene
{
    IReadOnlyCollection<TGene> Genes { get; }

    double TargetFunctionValue { get; }

    double FitnessFunctionValue { get; }

    IIndividual<TGene> Mutate();

    IIndividual<TGene> Crossbreed(IIndividual<TGene> individual);
}

using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Algorithms.Contracts.Genetic.Operators;

namespace GSOP.Domain.Algorithms.Genetic.Models;

public class Individual<TGene> : IIndividual<TGene> where TGene : IGene
{
    private readonly ICrossoverOperator<TGene> _crossoverOperator;
    private readonly IMutationOperator<TGene> _mutationOperator;
    private readonly ITargetFunctionCalculator<IIndividual<TGene>> _targetFunctionCalculator;
    private readonly IFitnessCalculator<IIndividual<TGene>> _fitnessCalculator;

    public IReadOnlyCollection<TGene> Genes { get; }

    public double TargetFunctionValue { get; }

    public double FitnessFunctionValue { get; }

    public Individual(
        ICrossoverOperator<TGene> crossoverOperator,
        IMutationOperator<TGene> mutationOperator,
        ITargetFunctionCalculator<IIndividual<TGene>> targetFunctionCalculator,
        IFitnessCalculator<IIndividual<TGene>> fitnessCalculator,
        IReadOnlyCollection<TGene> genes)
    {
        _crossoverOperator = crossoverOperator;
        _mutationOperator = mutationOperator;
        _targetFunctionCalculator = targetFunctionCalculator;
        _fitnessCalculator = fitnessCalculator;

        Genes = genes;
        TargetFunctionValue = _targetFunctionCalculator.Calculate(this);
        FitnessFunctionValue = _fitnessCalculator.Calculate(this);
    }

    public IIndividual<TGene> Crossbreed(IIndividual<TGene> individual)
    {
        var newGenes = _crossoverOperator.CreateChild(Genes, individual.Genes);

        return new Individual<TGene>(_crossoverOperator, _mutationOperator, _targetFunctionCalculator, _fitnessCalculator, newGenes);
    }

    public IIndividual<TGene> Mutate()
    {
        var newGenes = _mutationOperator.Mutate(Genes);

        return new Individual<TGene>(_crossoverOperator, _mutationOperator, _targetFunctionCalculator, _fitnessCalculator, newGenes);
    }
}

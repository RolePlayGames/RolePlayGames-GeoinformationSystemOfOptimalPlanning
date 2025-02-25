using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Optimization.FitnessFunctionCalculators;

public class MinFitnessCalculator<TGene> : IFitnessCalculator<IIndividual<TGene>> where TGene : IGene
{
    private readonly ITargetFunctionCalculator<IIndividual<TGene>> _targetFunctionCalculator;

    public MinFitnessCalculator(ITargetFunctionCalculator<IIndividual<TGene>> targetFunctionCalculator)
    {
        _targetFunctionCalculator = targetFunctionCalculator ?? throw new ArgumentNullException(nameof(targetFunctionCalculator));
    }

    public double Calculate(IIndividual<TGene> individual)
    {
        return -_targetFunctionCalculator.Calculate(individual);
    }
}

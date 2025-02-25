using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Optimization.FitnessFunctionCalculators;

public class MaxFitnessCalculator<TIndividual, TGene> : IFitnessCalculator<TIndividual> where TGene : IGene where TIndividual : IIndividual<TGene>
{
    private readonly ITargetFunctionCalculator<TIndividual> _targetFunctionCalculator;

    public MaxFitnessCalculator(ITargetFunctionCalculator<TIndividual> targetFunctionCalculator)
    {
        _targetFunctionCalculator = targetFunctionCalculator ?? throw new ArgumentNullException(nameof(targetFunctionCalculator));
    }

    public double Calculate(TIndividual individual)
    {
        return _targetFunctionCalculator.Calculate(individual);
    }
}

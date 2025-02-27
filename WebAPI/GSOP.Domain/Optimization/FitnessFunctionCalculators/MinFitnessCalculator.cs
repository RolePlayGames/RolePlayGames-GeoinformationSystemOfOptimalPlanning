using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Contracts.Optimization.Models;

namespace GSOP.Domain.Optimization.FitnessFunctionCalculators;

public class MinFitnessCalculator : IFitnessCalculator<ProductionPlan>
{
    private readonly ITargetFunctionCalculator<ProductionPlan> _targetFunctionCalculator;

    public MinFitnessCalculator(ITargetFunctionCalculator<ProductionPlan> targetFunctionCalculator)
    {
        _targetFunctionCalculator = targetFunctionCalculator ?? throw new ArgumentNullException(nameof(targetFunctionCalculator));
    }

    public double Calculate(ProductionPlan individual)
    {
        return -_targetFunctionCalculator.Calculate(individual);
    }
}

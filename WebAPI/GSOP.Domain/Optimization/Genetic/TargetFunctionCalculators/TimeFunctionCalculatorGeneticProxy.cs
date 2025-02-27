using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Contracts.Optimization.Genetic.Models;
using GSOP.Domain.Contracts.Optimization.Models;
using GSOP.Domain.Optimization.Genetic.IndividualConverters;

namespace GSOP.Domain.Optimization.Genetic.TargetFunctionCalculators;

public class TimeFunctionCalculatorGeneticProxy : ITargetFunctionCalculator<IIndividual<OrderPosition>>
{
    private readonly IIndividualConverter _individualConverter;
    private readonly ITargetFunctionCalculator<ProductionPlan> _targetFunctionCalculator;

    public TimeFunctionCalculatorGeneticProxy(IIndividualConverter individualConverter, ITargetFunctionCalculator<ProductionPlan> targetFunctionCalculator)
    {
        _individualConverter = individualConverter;
        _targetFunctionCalculator = targetFunctionCalculator;
    }

    public double Calculate(IIndividual<OrderPosition> dicision)
    {
        return _targetFunctionCalculator.Calculate(_individualConverter.ConvertToProductionPlan(dicision));
    }
}

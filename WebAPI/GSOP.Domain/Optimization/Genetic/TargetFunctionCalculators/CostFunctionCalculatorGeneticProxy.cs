using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Contracts.Optimization.Genetic.Models;
using GSOP.Domain.Contracts.Optimization.Models;
using GSOP.Domain.Optimization.Genetic.IndividualConverters;

namespace GSOP.Domain.Optimization.Genetic.TargetFunctionCalculators;

public class CostFunctionCalculatorGeneticProxy : ITargetFunctionCalculator<IIndividual<OrderPosition>>
{
    private readonly ITargetFunctionCalculator<ProductionPlan> _targetFunctionCalculator;
    private readonly IIndividualConverter _individualConverter;

    public CostFunctionCalculatorGeneticProxy(ITargetFunctionCalculator<ProductionPlan> targetFunctionCalculator, IIndividualConverter individualConverter)
    {
        _targetFunctionCalculator = targetFunctionCalculator;
        _individualConverter = individualConverter;
    }

    public double Calculate(IIndividual<OrderPosition> dicision)
    {
        return _targetFunctionCalculator.Calculate(_individualConverter.ConvertToProductionPlan(dicision));
    }
}

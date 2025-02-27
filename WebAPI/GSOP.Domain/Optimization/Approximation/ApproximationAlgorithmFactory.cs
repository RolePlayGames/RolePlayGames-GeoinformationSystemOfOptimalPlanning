using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Contracts.Optimization.Approximation;
using GSOP.Domain.Contracts.Optimization.Models;
using GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Time;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using System.Collections.Frozen;

namespace GSOP.Domain.Optimization.Approximation;

public class ApproximationAlgorithmFactory : IApproximationAlgorithmFactory
{
    public IOptimizationAlgorithm<ProductionPlan> CreateAlgorithm(IReadOnlyCollection<IProductionLine> productionLines, IReadOnlyCollection<IOrder> orders, IOrderExcecutionTimeCalculator orderExcecutionTimeCalculator)
    {
        var filmTypeChanges = productionLines.ToFrozenDictionary(x => x, x => x.FilmTypeChangeRules);

        return new ApproximationAlgorithm(productionLines, orders, orderExcecutionTimeCalculator, filmTypeChanges);                      
    }                                                             
}

using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Contracts.Optimization.Models;
using GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Time;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;

namespace GSOP.Domain.Contracts.Optimization.Approximation;

public interface IApproximationAlgorithmFactory
{
    IOptimizationAlgorithm<ProductionPlan> CreateAlgorithm(IReadOnlyCollection<IProductionLine> productionLines, IReadOnlyCollection<IOrder> orders, IOrderExcecutionTimeCalculator orderExcecutionTimeCalculator);
}

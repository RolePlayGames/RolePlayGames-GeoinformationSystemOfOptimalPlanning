using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Contracts.Optimization.Genetic.Models;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Optimization.Genetic.Operators.Crossovers;
using GSOP.Domain.Optimization.TargetFunctionCalculators.Cost.Base;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Cost;

public class CostFunctionCalculator : ITargetFunctionCalculator<IIndividual<OrderPosition>>
{
    private readonly IProductionLineQueueCostCalculator _productionLineQueueCostCalculator;

    public CostFunctionCalculator(IProductionLineQueueCostCalculator productionLineQueueCostCalculator)
    {
        _productionLineQueueCostCalculator = productionLineQueueCostCalculator ?? throw new ArgumentNullException(nameof(productionLineQueueCostCalculator));
    }

    public double Calculate(IIndividual<OrderPosition> individual)
    {
        var lineQueues = new Dictionary<IProductionLine, SortedList<int, IOrder>>();

        foreach (var gene in individual.Genes)
        {
            lineQueues.GetOrCreate(gene.ProductionLine).Add(gene.Position, gene.Order);
        }

        return lineQueues.Sum(x => _productionLineQueueCostCalculator.Calculate(new() { ProductionLine = x.Key, Orders = x.Value.Values }));
    }
}

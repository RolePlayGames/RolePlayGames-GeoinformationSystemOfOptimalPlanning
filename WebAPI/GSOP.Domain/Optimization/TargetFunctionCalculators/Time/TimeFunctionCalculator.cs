using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Contracts.Optimization.Genetic.Models;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Optimization.Genetic.Operators.Crossovers;
using GSOP.Domain.Optimization.TargetFunctionCalculators.Time.Base;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Time;

public class TimeFunctionCalculator : ITargetFunctionCalculator<IIndividual<OrderPosition>>
{
    private readonly IProductionLineQueueTimeCalculator _productionLineQueueTimeCalculator;

    public TimeFunctionCalculator(IProductionLineQueueTimeCalculator productionLineQueueTimeCalculator)
    {
        _productionLineQueueTimeCalculator = productionLineQueueTimeCalculator ?? throw new ArgumentNullException(nameof(productionLineQueueTimeCalculator));
    }

    public double Calculate(IIndividual<OrderPosition> individual)
    {
        var lineQueues = new Dictionary<IProductionLine, SortedList<int, IOrder>>();

        foreach (var gene in individual.Genes)
        {
            lineQueues.GetOrCreate(gene.ProductionLine).Add(gene.Position, gene.Order);
        }

        return lineQueues.Max(x => _productionLineQueueTimeCalculator.Calculate(new() { ProductionLine = x.Key, Orders = x.Value.Values }));
    }
}

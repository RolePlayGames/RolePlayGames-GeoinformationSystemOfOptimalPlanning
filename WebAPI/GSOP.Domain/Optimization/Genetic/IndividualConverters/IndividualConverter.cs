using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Contracts.Optimization.Genetic.Models;
using GSOP.Domain.Contracts.Optimization.Models;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Optimization.Genetic.Operators.Crossovers;

namespace GSOP.Domain.Optimization.Genetic.IndividualConverters;

public class IndividualConverter : IIndividualConverter
{
    public ProductionPlan ConvertToProductionPlan(IIndividual<OrderPosition> individual)
    {
        var lineQueues = new Dictionary<IProductionLine, List<IOrder>>();

        foreach (var gene in individual.Genes)
        {
            lineQueues.GetOrCreate(gene.ProductionLine).Add(gene.Order);
        }

        return new() { ProductionLineQueues = lineQueues.Select(x => new ProductionLineQueue { ProductionLine = x.Key, Orders = x.Value}).ToList() };
    }
}

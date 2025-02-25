using GSOP.Domain.Contracts.Optimization.Genetic.Models;
using GSOP.Domain.Algorithms.Contracts.Genetic.Operators;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Optimization.Genetic.Operators.Crossovers;

namespace GSOP.Domain.Optimization.Genetic.Operators.Mutations;

public class InversionMutationOperator : IMutationOperator<OrderPosition>
{
    private readonly Random _random;

    public InversionMutationOperator(Random random)
    {
        _random = random ?? throw new ArgumentNullException(nameof(random));
    }

    public IReadOnlyCollection<OrderPosition> Mutate(IReadOnlyCollection<OrderPosition> genes)
    {
        var newGenes = genes.ToList();

        var start = _random.Next(0, newGenes.Count);
        var end = _random.Next(0, newGenes.Count);

        if (start > end)
        {
            (start, end) = (end, start);
        }

        var length = end - start + 1;
        var sublist = newGenes.GetRange(start, length);
        sublist.Reverse();

        newGenes.RemoveRange(start, length);
        newGenes.InsertRange(start, sublist);

        var queues = new Dictionary<IProductionLine, List<(int Position, IOrder Order)>>();

        foreach (var gene in newGenes)
        {
            queues.GetOrCreate(gene.ProductionLine)
                .Add((gene.Position, gene.Order));
        }

        return queues.SelectMany(queue => queue.Value.OrderBy(position => position.Position).Select((position, index) => new OrderPosition { Order = position.Order, Position = index, ProductionLine = queue.Key })).ToList();
    }
}

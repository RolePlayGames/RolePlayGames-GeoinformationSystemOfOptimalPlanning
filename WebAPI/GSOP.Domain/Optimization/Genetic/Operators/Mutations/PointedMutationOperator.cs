using GSOP.Domain.Contracts.Optimization.Genetic.Models;
using GSOP.Domain.Algorithms.Contracts.Genetic.Operators;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Optimization.Genetic.Operators.Crossovers;
using GSOP.Domain.Algorithms.Genetic.Extensions;

namespace GSOP.Domain.Optimization.Genetic.Operators.Mutations;

public class PointedMutationOperator : IMutationOperator<OrderPosition>
{
    private readonly Random _random;
    private readonly double _probability;

    public PointedMutationOperator(Random random, double probability)
    {
        if (probability < 0 || probability > 1)
            throw new ArgumentOutOfRangeException(nameof(probability), "Probability should be in range between 0 and 1.");

        _probability = probability;
        _random = random ?? throw new ArgumentNullException(nameof(random), "Random generator should be not null.");
    }

    public IReadOnlyCollection<OrderPosition> Mutate(IReadOnlyCollection<OrderPosition> genes)
    {
        var queues = new Dictionary<IProductionLine, List<(int Position, IOrder Order)>>();
        var ordersToMutate = new List<IOrder>();

        for (var i = 0; i < genes.Count; i++)
        {
            var gene = genes.ElementAt(i);

            if (_random.NextDouble() < _probability)
            {
                ordersToMutate.Add(gene.Order);
            }
            else
            {
                queues.GetOrCreate(gene.ProductionLine)
                    .Add((gene.Position, gene.Order));
            }
        }

        var lines = queues.Keys;

        foreach (var order in ordersToMutate)
        {
            var line = _random.NextElement(lines);
            var queue = queues[line];
            var position = _random.Next(queue.Count);

            queue.Add((position, order));
        }

        var result = queues.SelectMany(queue => queue.Value.OrderBy(position => position.Position).Select((position, index) => new OrderPosition { Order = position.Order, Position = index, ProductionLine = queue.Key })).ToList();

        return result;
    }
}

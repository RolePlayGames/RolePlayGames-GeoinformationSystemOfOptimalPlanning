using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Algorithms.Contracts.Genetic.Operators;
using GSOP.Domain.Contracts.Optimization.Genetic.Models;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;

namespace GSOP.Domain.Optimization.Genetic.Operators.Crossovers;

public class PointedCrossoverOperator : ICrossoverOperator<OrderPosition>
{
    private readonly Random _random;

    public PointedCrossoverOperator(Random random)
    {
        _random = random ?? throw new ArgumentNullException(nameof(random), "Random should be in range between 0 and 1.");
    }

    public IReadOnlyCollection<OrderPosition> CreateChild(IReadOnlyCollection<OrderPosition> firstParent, IReadOnlyCollection<OrderPosition> secondParent)
    {
        if (firstParent.Count < 2)
            return firstParent;

        var point1 = _random.Next(0, firstParent.Count);
        var point2 = _random.Next(0, firstParent.Count);

        if (point1 > point2)
        {
            (point1, point2) = (point2, point1);
        }

        var queues = new Dictionary<IProductionLine, List<(int Position, IOrder Order)>>();
        var orders = new HashSet<IOrder>(firstParent.Count);

        var childGenes = secondParent.Skip(point1).Take(point2 - point1).Concat(firstParent.Take(point1)).Concat(firstParent.Skip(point2));

        foreach (var gene in childGenes)
        {
            if (orders.Contains(gene.Order))
                continue;

            orders.Add(gene.Order);
            queues.GetOrCreate(gene.ProductionLine)
                .Add((gene.Position, gene.Order));
        }

        var missingGenes = secondParent.Where(x => !orders.Contains(x.Order));

        foreach (var gene in missingGenes)
        {
            queues.GetOrCreate(gene.ProductionLine)
                .Add((gene.Position, gene.Order));
        }

        return queues.SelectMany(queue => queue.Value.OrderBy(position => position.Position).Select((position, index) => new OrderPosition { Order = position.Order, Position = index, ProductionLine = queue.Key })).ToList();
    }
}

public static class DictionaryExtension
{
    public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) where TValue : new()
    {
        if (!dict.TryGetValue(key, out var val))
        {
            val = new TValue();
            dict.Add(key, val);
        }

        return val;
    }
}

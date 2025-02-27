using GSOP.Domain.Contracts.Optimization.Genetic.Models;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Algorithms.Contracts.Genetic.Operators;

namespace GSOP.Domain.Optimization.Genetic.Operators.Crossovers;

public class MultipointedCrossoverOperator : ICrossoverOperator<OrderPosition>
{
    private readonly int _pointCount;
    private readonly Random _random;

    public MultipointedCrossoverOperator(int pointCount, Random random)
    {
        if (pointCount < 2)
            throw new ArgumentOutOfRangeException(nameof(pointCount), "Point count should be greater than or equal to 1");

        _pointCount = pointCount;
        _random = random ?? throw new ArgumentNullException(nameof(random));
    }

    public IReadOnlyCollection<OrderPosition> CreateChild(IReadOnlyCollection<OrderPosition> firstParent, IReadOnlyCollection<OrderPosition> secondParent)
    {
        if (firstParent.Count < _pointCount)
            return firstParent;

        var crossoverPoints = Enumerable.Range(0, firstParent.Count).OrderBy(_ => _random.Next()).Take(_pointCount).OrderBy(i => i).ToList();
        crossoverPoints.Insert(0, 0); 
        crossoverPoints.Add(firstParent.Count);

        var orders = new HashSet<IOrder>();
        var queues = new Dictionary<IProductionLine, List<(int Position, IOrder Order)>>();
        var useFirstParent = true;

        for (var i = 0; i < crossoverPoints.Count - 1; i++)
        {
            var start = crossoverPoints[i];
            var end = crossoverPoints[i + 1];
            var currentParent = useFirstParent ? firstParent : secondParent;

            for (var j = start; j < end; j++)
            {
                var gene = currentParent.ElementAt(j);

                if (orders.Contains(gene.Order))
                    continue;

                orders.Add(gene.Order);
                queues.GetOrCreate(gene.ProductionLine)
                    .Add((gene.Position, gene.Order));
            }

            useFirstParent = !useFirstParent;
        }

        var missingGenes = secondParent.Where(x => !orders.Contains(x.Order));

        foreach (var gene in missingGenes)
        {
            queues.GetOrCreate(gene.ProductionLine)
                .Add((gene.Position, gene.Order));
        }

        var result = queues.SelectMany(queue => queue.Value.OrderBy(position => position.Position).Select((position, index) => new OrderPosition { Order = position.Order, Position = index, ProductionLine = queue.Key })).ToList();

        return result;
    }
}

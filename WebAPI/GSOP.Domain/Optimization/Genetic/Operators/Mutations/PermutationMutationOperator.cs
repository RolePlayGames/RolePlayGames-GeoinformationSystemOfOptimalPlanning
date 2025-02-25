using GSOP.Domain.Contracts.Optimization.Genetic.Models;
using GSOP.Domain.Algorithms.Contracts.Genetic.Operators;

namespace GSOP.Domain.Optimization.Genetic.Operators.Mutations;

public class PermutationMutationOperator : IMutationOperator<OrderPosition>
{
    private readonly Random _random;
    private readonly double _probability;

    public PermutationMutationOperator(Random random, double probability)
    {
        if (probability < 0 || probability > 1)
            throw new ArgumentOutOfRangeException(nameof(probability), "Probability should be in range between 0 and 1.");

        _probability = probability;
        _random = random ?? throw new ArgumentNullException(nameof(random));
    }

    public IReadOnlyCollection<OrderPosition> Mutate(IReadOnlyCollection<OrderPosition> genes)
    {
        var newGenes = genes.ToList();

        for (var i = 0; i < newGenes.Count; i++)
        {
            if (_random.NextDouble() < _probability)
            {
                var swapIndex = _random.Next(newGenes.Count);
                var oldGene = newGenes[i];

                newGenes[i] = new OrderPosition { Order = newGenes[i].Order, Position = newGenes[swapIndex].Position, ProductionLine = newGenes[swapIndex].ProductionLine };
                newGenes[swapIndex] = new OrderPosition { Order = newGenes[swapIndex].Order, Position = oldGene.Position, ProductionLine = oldGene.ProductionLine };
            }
        }

        return newGenes.GroupBy(x => x.ProductionLine).SelectMany(queue => queue.OrderBy(x => x.Position).Select((position, index) => new OrderPosition { Order = position.Order, Position = index, ProductionLine = queue.Key })).ToList();
    }
}

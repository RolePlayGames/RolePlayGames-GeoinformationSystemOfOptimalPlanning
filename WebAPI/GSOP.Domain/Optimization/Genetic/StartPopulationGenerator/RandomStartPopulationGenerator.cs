using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Genetic;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Algorithms.Contracts.Genetic.Operators;
using GSOP.Domain.Algorithms.Genetic.Extensions;
using GSOP.Domain.Algorithms.Genetic.Models;
using GSOP.Domain.Contracts.Optimization.Genetic.Models;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using System.Collections.Frozen;

namespace GSOP.Domain.Optimization.Genetic.StartPopulationGenerator;

public class RandomStartPopulationGenerator : IStartPopulationFactory<OrderPosition>
{
    private readonly Random _random;
    private readonly ICollection<IProductionLine> _extruders;
    private readonly ICollection<IOrder> _orders;

    private readonly IIndividualsSelector<OrderPosition> _mutationOperatorSelector;
    private readonly ICrossoverOperatorSelector<OrderPosition> _crossoverOperatorSelector;
    private readonly IPopulationSelector<OrderPosition> _populationSelector;
    private readonly IBestSelector<OrderPosition> _bestSelector;

    private readonly ICrossoverOperator<OrderPosition> _crossoverOperator;
    private readonly IMutationOperator<OrderPosition> _mutationOperator;
    private readonly ITargetFunctionCalculator<IIndividual<OrderPosition>> _targetFunctionCalculator;
    private readonly IFitnessCalculator<IIndividual<OrderPosition>> _fitnessCalculator;

    private readonly int _count;

    public RandomStartPopulationGenerator(
        Random random,
        ICollection<IProductionLine> extruders,
        ICollection<IOrder> orders,
        IIndividualsSelector<OrderPosition> mutationOperatorSelector,
        ICrossoverOperatorSelector<OrderPosition> crossoverOperatorSelector,
        IPopulationSelector<OrderPosition> populationSelector,
        IBestSelector<OrderPosition> bestSelector,
        ICrossoverOperator<OrderPosition> crossoverOperator,
        IMutationOperator<OrderPosition> mutationOperator,
        ITargetFunctionCalculator<IIndividual<OrderPosition>> targetFunctionCalculator,
        IFitnessCalculator<IIndividual<OrderPosition>> fitnessCalculator,
        int count)
    {
        _random = random;
        _extruders = extruders;
        _orders = orders;
        _mutationOperatorSelector = mutationOperatorSelector;
        _crossoverOperatorSelector = crossoverOperatorSelector;
        _populationSelector = populationSelector;
        _bestSelector = bestSelector;
        _crossoverOperator = crossoverOperator;
        _mutationOperator = mutationOperator;
        _targetFunctionCalculator = targetFunctionCalculator;
        _fitnessCalculator = fitnessCalculator;
        _count = count;
    }

    public IPopulation<OrderPosition> CreateStartPopulation()
    {
        var result = new IIndividual<OrderPosition>[_count];

        for (var i = 0; i < _count; i++)
        {
            result[i] = GenerateRandomIndividual();
        }

        return new Population<OrderPosition>(_mutationOperatorSelector, _crossoverOperatorSelector, _populationSelector, _bestSelector, result);
    }

    private IIndividual<OrderPosition> GenerateRandomIndividual()
    {
        var productionLinesQueue = _extruders.ToFrozenDictionary(x => x, x => new List<IOrder>(_orders.Count));

        foreach (var order in _random.NextElements(_orders))
        {
            var productionLine = _random.NextElement(_extruders);
            productionLinesQueue[productionLine].Add(order);
        }

        var genes = productionLinesQueue.SelectMany(queue => queue.Value.Select((order, index) => new OrderPosition { Order = order, Position = index, ProductionLine = queue.Key })).ToList();

        return new Individual<OrderPosition>(_crossoverOperator, _mutationOperator, _targetFunctionCalculator, _fitnessCalculator, genes);
    }
}

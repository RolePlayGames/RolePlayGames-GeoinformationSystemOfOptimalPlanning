using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Genetic;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Algorithms.Genetic;
using GSOP.Domain.Algorithms.Genetic.IndividualsSelectors;
using GSOP.Domain.Algorithms.Genetic.IndividualsSelectors.ProbabilityIndividualsSelectors;
using GSOP.Domain.Algorithms.Genetic.Operators.Crossovers.Selectors;
using GSOP.Domain.Algorithms.Genetic.Probabilities;
using GSOP.Domain.Contracts.Optimization.Genetic;
using GSOP.Domain.Contracts.Optimization.Genetic.Models;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Optimization.Genetic.Operators.Crossovers;
using GSOP.Domain.Optimization.Genetic.Operators.Mutations;
using GSOP.Domain.Optimization.Genetic.StartPopulationGenerator;

namespace GSOP.Domain.Optimization.Genetic;

public class GeneticAlgorithmFactory : IGeneticAlgorithmFactory
{
    private readonly Random _random = new();
    private readonly IGeneticFinalConditionCheckerFactory _geneticFinalConditionCheckerFactory;

    public GeneticAlgorithmFactory(IGeneticFinalConditionCheckerFactory geneticFinalConditionCheckerFactory)
    {
        _geneticFinalConditionCheckerFactory = geneticFinalConditionCheckerFactory;
    }

    public IGeneticAlgorithmsDecisions<OrderPosition> CreateAlgorithm(
        IReadOnlyCollection<IProductionLine> productionLines,
        IReadOnlyCollection<IOrder> orders,
        ITargetFunctionCalculator<IIndividual<OrderPosition>> targetFunctionCalculator,
        IFitnessCalculator<IIndividual<OrderPosition>> fitnessCalculator,
        GeneticAlgorithmOptions options,
        GeneticFinalCheckerConditions geneticFinalCheckerConditions)
    {

        var randomProbabilityChecker = new RandomProbabilityChecker(_random);
        var bestSelector = new BestSelector<OrderPosition>();

        var mutationOperatorSelector = new NonLinearRankingIndividualsSelector<OrderPosition>(options.MutationCoefficient, randomProbabilityChecker, options.MutationSelectionCount);
        var crossoverOperatorSelector = new OutbreedingCrossoverOperatorSelector<OrderPosition>(new RouletteIndividualsSelector<OrderPosition>(randomProbabilityChecker, options.CrossoverSelectionCount));
        var populationSelector = new PopulationSelector<OrderPosition>(new TournamentIndividualsSelector<OrderPosition>(bestSelector, options.PopulationSelectorTeamCapacity));
        var crossoverOperator = new MultipointedCrossoverOperator(options.CrossoverPointsCount, _random);
        var mutationOperator = new PointedMutationOperator(_random, options.PointedMutationProbability);

        var startPopulationFactory = new RandomStartPopulationGenerator(
            _random,
            productionLines,
            orders,
            mutationOperatorSelector,
            crossoverOperatorSelector,
            populationSelector,
            bestSelector,
            crossoverOperator,
            mutationOperator,
            targetFunctionCalculator,
            fitnessCalculator,
            options.StartPopulationsCount);

        var finalConditionCheckers = _geneticFinalConditionCheckerFactory.CreateFinalConditionCheckers(geneticFinalCheckerConditions);

        return new GeneticAlgorithm<OrderPosition>(startPopulationFactory, finalConditionCheckers);
    }
}

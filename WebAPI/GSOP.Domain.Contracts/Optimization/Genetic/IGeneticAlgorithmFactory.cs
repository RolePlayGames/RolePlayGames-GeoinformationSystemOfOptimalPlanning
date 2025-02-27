using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Genetic;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Contracts.Optimization.Genetic.Models;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;

namespace GSOP.Domain.Contracts.Optimization.Genetic;

public record GeneticAlgorithmOptions(
    double MutationCoefficient,
    int MutationSelectionCount,
    int CrossoverSelectionCount,
    int PopulationSelectorTeamCapacity,
    int CrossoverPointsCount,
    double PointedMutationProbability,
    int StartPopulationsCount);

public interface IGeneticAlgorithmFactory
{
    IGeneticAlgorithmsDecisions<OrderPosition> CreateAlgorithm(
        IReadOnlyCollection<IProductionLine> productionLines,
        IReadOnlyCollection<IOrder> orders,
        ITargetFunctionCalculator<IIndividual<OrderPosition>> targetFunctionCalculator,
        IFitnessCalculator<IIndividual<OrderPosition>> fitnessCalculator,
        GeneticAlgorithmOptions options,
        GeneticFinalCheckerConditions geneticFinalCheckerConditions);
}

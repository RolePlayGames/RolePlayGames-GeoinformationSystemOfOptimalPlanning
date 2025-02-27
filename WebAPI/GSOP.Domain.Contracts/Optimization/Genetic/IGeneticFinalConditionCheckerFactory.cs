using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Contracts.Optimization.Genetic.Models;

namespace GSOP.Domain.Contracts.Optimization.Genetic;

public interface IGeneticFinalConditionCheckerFactory
{
    IReadOnlyCollection<IFinalConditionChecker<IPopulation<OrderPosition>>> CreateFinalConditionCheckers(GeneticFinalCheckerConditions conditions);
}

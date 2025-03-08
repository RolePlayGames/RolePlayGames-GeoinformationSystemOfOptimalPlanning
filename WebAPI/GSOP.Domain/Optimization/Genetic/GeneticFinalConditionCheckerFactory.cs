using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Contracts.Optimization.Genetic;
using GSOP.Domain.Contracts.Optimization.Genetic.Models;
using GSOP.Domain.Optimization.FinalConditionChecker;
using GSOP.Domain.Optimization.Genetic.FinalConditionCheckers;

namespace GSOP.Domain.Optimization.Genetic;

public class GeneticFinalConditionCheckerFactory : IGeneticFinalConditionCheckerFactory
{
    public IReadOnlyCollection<IFinalConditionChecker<IPopulation<OrderPosition>>> CreateFinalConditionCheckers(GeneticFinalCheckerConditions conditions)
    {
        var result = new List<IFinalConditionChecker<IPopulation<OrderPosition>>>();

        if (conditions.TimeoutDelay.HasValue)
        {
            result.Add(new TimeFinalConditionChecker<IPopulation<OrderPosition>>(conditions.TimeoutDelay.Value));
        }
        else
        {
            result.Add(new TimeFinalConditionChecker<IPopulation<OrderPosition>>(TimeSpan.FromMinutes(3)));
        }

        if (conditions.IterationsCount.HasValue)
        {
            result.Add(new IterationsFinalConditionChecker<IPopulation<OrderPosition>>(conditions.IterationsCount.Value));
        }

        if (conditions.GenerationsCount.HasValue)
        {
            result.Add(new PopulationFitnessDescendingFinalConditionChecker<OrderPosition>(conditions.GenerationsCount.Value));
        }

        return result;
    }
}

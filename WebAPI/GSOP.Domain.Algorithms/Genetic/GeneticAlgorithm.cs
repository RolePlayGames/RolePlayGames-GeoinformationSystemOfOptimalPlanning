using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Genetic;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Algorithms.Genetic.Extensions;

namespace GSOP.Domain.Algorithms.Genetic;

public class GeneticAlgorithm<TGene> : IGeneticAlgorithm<TGene>, IGeneticAlgorithmsDecisions<TGene> where TGene : IGene
{
    private readonly int _approximationInclude = 100;
    private readonly IStartPopulationFactory<TGene> _startPopulationFactory;
    private readonly IReadOnlyCollection<IFinalConditionChecker<IPopulation<TGene>>> _finalConditionCheckers;
    private readonly Func<IIndividual<TGene>> _approximationFactory;

    public GeneticAlgorithm(IStartPopulationFactory<TGene> startPopulationFactory, IReadOnlyCollection<IFinalConditionChecker<IPopulation<TGene>>> finalConditionCheckers, Func<IIndividual<TGene>> approximationFactory)
    {
        _startPopulationFactory = startPopulationFactory;
        _finalConditionCheckers = finalConditionCheckers;
        _approximationFactory = approximationFactory;
    }

    IIndividual<TGene>? IOptimizationAlgorithm<IIndividual<TGene>>.GetResolve()
    {
        return ((IOptimizationAlgorithm<IEnumerable<IIndividual<TGene>>>)this).GetResolve().LastOrDefault();
    }

    IEnumerable<IIndividual<TGene>> IOptimizationAlgorithm<IEnumerable<IIndividual<TGene>>>.GetResolve()
    {
        IIndividual<TGene>? bestResolve = null;

        foreach (var resolve in GetResolvesInternal())
        {
            if (bestResolve is null || bestResolve.FitnessFunctionValue < resolve.FitnessFunctionValue)
            {
                bestResolve = resolve;
                yield return resolve;
            }
        }
    }

    private IEnumerable<IIndividual<TGene>> GetResolvesInternal()
    {
        var i = 0;
        var currentPopulation = _startPopulationFactory.CreateStartPopulation();

        _finalConditionCheckers.ForEach(x => x.Begin());

        while (!_finalConditionCheckers.Any(x => x.IsStateFinal(currentPopulation)))
        {
            if (++i == _approximationInclude)
            {
                currentPopulation = currentPopulation.IncludeApproximation(_approximationFactory());
            }

            currentPopulation = currentPopulation.Reproduction()
                .Mutation()
                .Selection();

            var best = currentPopulation.Best;

            if (best is not null)
                yield return best;
        }
    }
}

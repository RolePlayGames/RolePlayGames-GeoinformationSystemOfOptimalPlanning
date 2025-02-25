using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Genetic;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Algorithms.Genetic.Extensions;

namespace GSOP.Domain.Algorithms.Genetic;

public class GeneticAlgorithm<TGene> : IGeneticAlgorithm<TGene>, IGeneticAlgorithmsDecisions<TGene> where TGene : IGene
{
    private readonly IStartPopulationFactory<TGene> _startPopulationFactory;
    private readonly IReadOnlyCollection<IFinalConditionChecker<IPopulation<TGene>>> _finalConditionCheckers;

    public GeneticAlgorithm(IStartPopulationFactory<TGene> startPopulationFactory, IReadOnlyCollection<IFinalConditionChecker<IPopulation<TGene>>> finalConditionCheckers)
    {
        _startPopulationFactory = startPopulationFactory;
        _finalConditionCheckers = finalConditionCheckers;
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
        var currentPopulation = _startPopulationFactory.CreateStartPopulation();

        _finalConditionCheckers.ForEach(x => x.Begin());

        while (!_finalConditionCheckers.Any(x => x.IsStateFinal(currentPopulation)))
        {
            currentPopulation = currentPopulation.Reproduction()
                .Mutation()
                .Selection();

            var best = currentPopulation.Best;

            if (best is not null)
                yield return best;
        }
    }
}

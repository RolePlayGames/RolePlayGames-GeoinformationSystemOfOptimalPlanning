using GSOP.Domain.Algorithms.Contracts.Genetic.Operators;

namespace GSOP.Domain.Algorithms.Contracts.Genetic.Models;

public class Population<TGene> : IPopulation<TGene> where TGene : IGene
{
    private readonly IIndividualsSelector<TGene> _mutationOperatorSelector;
    private readonly ICrossoverOperatorSelector<TGene> _crossoverOperatorSelector;
    private readonly IPopulationSelector<TGene> _populationSelector;
    private readonly IBestSelector<TGene> _bestSelector;
    private readonly IReadOnlyCollection<IIndividual<TGene>> _population;

    public Population(
        IIndividualsSelector<TGene> mutationOperatorSelector,
        ICrossoverOperatorSelector<TGene> crossoverOperatorSelector,
        IPopulationSelector<TGene> populationSelector,
        IBestSelector<TGene> bestSelector,
        IReadOnlyCollection<IIndividual<TGene>> population)
    {
        _mutationOperatorSelector = mutationOperatorSelector;
        _crossoverOperatorSelector = crossoverOperatorSelector;
        _populationSelector = populationSelector;
        _bestSelector = bestSelector;
        _population = population;
    }

    public IIndividual<TGene>? Best => _bestSelector.SelectBestIndividual(_population);

    public IPopulation<TGene> IncludeApproximation(IIndividual<TGene> appriximation)
    {
        return new Population<TGene>(_mutationOperatorSelector, _crossoverOperatorSelector, _populationSelector, _bestSelector, _population.Concat([appriximation]).ToList());
    }

    public IPopulation<TGene> Mutation()
    {
        var individualsToMutate = _mutationOperatorSelector.SelectIndividuals(_population).ToList();

        var mutatedIndividuals = new IIndividual<TGene>[individualsToMutate.Count];
        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount / 2 };

        Parallel.For(0, individualsToMutate.Count, parallelOptions, i =>
        {
            mutatedIndividuals[i] = individualsToMutate[i].Mutate();
        });

        var newPopulation = _population.Concat(mutatedIndividuals).ToList();

        return new Population<TGene>(_mutationOperatorSelector, _crossoverOperatorSelector, _populationSelector, _bestSelector, newPopulation);
    }

    public IPopulation<TGene> Reproduction()
    {
        var parentsToCrossbreed = _crossoverOperatorSelector.SelectParents(_population).ToList();

        var newIndividuals = new IIndividual<TGene>[parentsToCrossbreed.Count];
        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount / 2 };

        Parallel.For(0, parentsToCrossbreed.Count, parallelOptions, i =>
        {
            newIndividuals[i] = parentsToCrossbreed[i].FirstParent.Crossbreed(parentsToCrossbreed[i].SecondParent);
        });

        var newPopulation = _population.Concat(newIndividuals).ToList();

        return new Population<TGene>(_mutationOperatorSelector, _crossoverOperatorSelector, _populationSelector, _bestSelector, newPopulation);
    }

    public IPopulation<TGene> Selection()
    {
        var individuals = _populationSelector.Select(_population);

        return new Population<TGene>(_mutationOperatorSelector, _crossoverOperatorSelector, _populationSelector, _bestSelector, individuals);
    }
}

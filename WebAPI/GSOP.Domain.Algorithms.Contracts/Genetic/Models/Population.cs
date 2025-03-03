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
        var newPopulation = _population.ToList();

        foreach (var individual in _mutationOperatorSelector.SelectIndividuals(_population))
        {
            newPopulation.Add(individual.Mutate());
        }

        return new Population<TGene>(_mutationOperatorSelector, _crossoverOperatorSelector, _populationSelector, _bestSelector, newPopulation);
    }

    public IPopulation<TGene> Reproduction()
    {
        var newPopulation = _population.ToList();

        foreach (var parents in _crossoverOperatorSelector.SelectParents(_population))
        {
            newPopulation.Add(parents.FirstParent.Crossbreed(parents.SecondParent));
        }

        return new Population<TGene>(_mutationOperatorSelector, _crossoverOperatorSelector, _populationSelector, _bestSelector, newPopulation);
    }

    public IPopulation<TGene> Selection()
    {
        var individuals = _populationSelector.Select(_population);

        return new Population<TGene>(_mutationOperatorSelector, _crossoverOperatorSelector, _populationSelector, _bestSelector, individuals);
    }
}

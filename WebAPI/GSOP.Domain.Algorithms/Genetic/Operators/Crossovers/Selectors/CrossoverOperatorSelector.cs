using GSOP.Domain.Algorithms.Contracts.Genetic;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Algorithms.Contracts.Genetic.Operators;

namespace GSOP.Domain.Algorithms.Genetic.Operators.Crossovers.Selectors;

public abstract class CrossoverOperatorSelector<TGene> : ICrossoverOperatorSelector<TGene> where TGene : IGene
{
    protected IIndividualsSelector<TGene> IndividualsSelector { get; }

    public CrossoverOperatorSelector(IIndividualsSelector<TGene> individualsSelector)
    {
        IndividualsSelector = individualsSelector ?? throw new ArgumentNullException(nameof(individualsSelector), "Individuals selector should not be null");
    }

    public IEnumerable<Parents<TGene>> SelectParents(IReadOnlyCollection<IIndividual<TGene>> individuals)
    {
        var parentsPull = IndividualsSelector.SelectIndividuals(individuals).ToList();

        foreach (var individual in parentsPull)
        {
            var secondParent = SelectSecondParent(individual, parentsPull);

            yield return new Parents<TGene> { FirstParent = individual, SecondParent = secondParent };
        }
    }

    protected abstract IIndividual<TGene> SelectSecondParent(IIndividual<TGene> firstParent, ICollection<IIndividual<TGene>> parentsPull);
}

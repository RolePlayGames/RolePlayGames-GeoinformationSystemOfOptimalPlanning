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
        var parentsPull = IndividualsSelector.SelectIndividuals(individuals).ToArray();

        var results = new Parents<TGene>[parentsPull.Length];
        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount / 2 };

        Parallel.For(0, results.Length, parallelOptions, i =>
        {
            var secondParent = SelectSecondParent(parentsPull[i], parentsPull);
            results[i] = new Parents<TGene> { FirstParent = parentsPull[i], SecondParent = secondParent };
        });

        return results;
    }

    protected abstract IIndividual<TGene> SelectSecondParent(IIndividual<TGene> firstParent, ICollection<IIndividual<TGene>> parentsPull);
}

using GSOP.Domain.Algorithms.Contracts.Genetic;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Algorithms.Genetic.Operators.Crossovers.Selectors;

public class PanmixianCrossoverOperatorSelector<TGene> : CrossoverOperatorSelector<TGene> where TGene : IGene
{
    protected Random Random { get; }

    public PanmixianCrossoverOperatorSelector(Random random, IIndividualsSelector<TGene> individualsSelector) : base(individualsSelector)
    {
        Random = random ?? throw new ArgumentNullException(nameof(random), "Random should not be null");
    }

    protected override IIndividual<TGene> SelectSecondParent(IIndividual<TGene> firstParent, ICollection<IIndividual<TGene>> parentsPull)
    {
        if (parentsPull.Count == 1)
            return firstParent;

        var index = Random.Next(0, parentsPull.Count - 1);

        if (parentsPull.ElementAt(index).Equals(firstParent))
            return parentsPull.ElementAt(index + 1);
        else
            return parentsPull.ElementAt(index);
    }
}

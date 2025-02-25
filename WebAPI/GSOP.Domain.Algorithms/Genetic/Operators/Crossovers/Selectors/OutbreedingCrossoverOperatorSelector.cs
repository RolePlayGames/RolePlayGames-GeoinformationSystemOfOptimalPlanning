using GSOP.Domain.Algorithms.Contracts.Genetic;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Algorithms.Genetic.Operators.Crossovers.Selectors;

public class OutbreedingCrossoverOperatorSelector<TGene> : CrossoverOperatorSelector<TGene> where TGene : IGene
{
    public OutbreedingCrossoverOperatorSelector(IIndividualsSelector<TGene> individualsSelector) : base(individualsSelector)
    {
    }

    protected override IIndividual<TGene> SelectSecondParent(IIndividual<TGene> firstParent, ICollection<IIndividual<TGene>> parentsPull)
    {
        var result = parentsPull.First();

        if (parentsPull.Count == 1)
            return firstParent;

        var firstParentFitnessFunctionValue = firstParent.FitnessFunctionValue;
        var minDifference = double.MinValue;

        foreach (var individual in parentsPull)
        {
            if (individual.Equals(firstParent))
                continue;

            var individualFitnessFunctionValue = individual.FitnessFunctionValue;
            var difference = Math.Abs(firstParentFitnessFunctionValue - individualFitnessFunctionValue);

            if (minDifference < difference)
            {
                result = individual;
                minDifference = difference;
            }
        }

        return result;
    }
}

using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Optimization.Genetic.FitnessFunctionCalculators;

public class MaxFitnessCalculator<TGene> : IFitnessCalculator<IIndividual<TGene>> where TGene : IGene
{
    public double Calculate(IIndividual<TGene> individual)
    {
        return individual.TargetFunctionValue;
    }
}

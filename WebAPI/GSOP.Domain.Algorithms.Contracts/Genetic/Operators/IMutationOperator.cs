using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Algorithms.Contracts.Genetic.Operators;

public interface IMutationOperator<TGene> where TGene : IGene
{
    /// <summary>
    /// Making mutation on genes collection
    /// </summary>
    /// <param name="genes">Individual genes</param>
    /// <returns>Changed genes</returns>
    IReadOnlyCollection<TGene> Mutate(IReadOnlyCollection<TGene> genes);
}

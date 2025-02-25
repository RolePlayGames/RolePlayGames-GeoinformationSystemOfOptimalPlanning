using GSOP.Domain.Algorithms.Contracts.Genetic.Models;

namespace GSOP.Domain.Algorithms.Contracts.Genetic.Operators;

public interface ICrossoverOperator<TGene> where TGene : IGene
{
    /// <summary>
    /// Creates child from parents
    /// </summary>
    /// <param name="firstParent">First parent</param>
    /// <param name="secondParent">Second parent</param>
    /// <returns>Child genes</returns>
    IReadOnlyCollection<TGene> CreateChild(IReadOnlyCollection<TGene> firstParent, IReadOnlyCollection<TGene> secondParent);
}

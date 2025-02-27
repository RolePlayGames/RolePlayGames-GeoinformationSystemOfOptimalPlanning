using GSOP.Domain.Algorithms.Contracts.Genetic;
using GSOP.Domain.Contracts.Optimization.Genetic.Models;

namespace GSOP.Domain.Contracts.Optimization.Genetic;

public interface IIndividualsSelectorFactory
{
    IIndividualsSelector<OrderPosition> CreateIndividualsSelector();
}

using GSOP.Domain.Algorithms.Contracts.Genetic;
using GSOP.Domain.Contracts.Optimization.Genetic;
using GSOP.Domain.Contracts.Optimization.Genetic.Models;

namespace GSOP.Domain.Optimization.Genetic;

public class IndividualsSelectorFactory : IIndividualsSelectorFactory
{
    public IIndividualsSelector<OrderPosition> CreateIndividualsSelector()
    {
        throw new NotImplementedException();
    }
}

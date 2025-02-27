using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Contracts.Optimization.Genetic.Models;
using GSOP.Domain.Contracts.Optimization.Models;

namespace GSOP.Domain.Optimization.Genetic.IndividualConverters;

public interface IIndividualConverter
{
    ProductionPlan ConvertToProductionPlan(IIndividual<OrderPosition> individual);
}

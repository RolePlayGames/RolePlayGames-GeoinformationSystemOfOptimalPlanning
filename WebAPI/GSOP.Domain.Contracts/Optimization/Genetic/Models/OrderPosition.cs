using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;

namespace GSOP.Domain.Contracts.Optimization.Genetic.Models;

public record OrderPosition : IGene
{
    public required IProductionLine ProductionLine { get; init; }

    public required IOrder Order { get; init; }

    public required int Position { get; init; }
}

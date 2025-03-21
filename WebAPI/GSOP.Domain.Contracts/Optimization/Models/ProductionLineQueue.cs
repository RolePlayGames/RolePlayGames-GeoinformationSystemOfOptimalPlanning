﻿using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;

namespace GSOP.Domain.Contracts.Optimization.Models;

public record ProductionLineQueue
{
    public required IProductionLine ProductionLine { get; init; }

    public required IReadOnlyCollection<IOrder> Orders { get; init; }
}

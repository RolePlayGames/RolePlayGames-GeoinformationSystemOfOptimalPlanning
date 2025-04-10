﻿using GSOP.Domain.Contracts.Optimization.Models;
using GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Time;

namespace GSOP.Domain.Optimization.TargetFunctionCalculators.Time;

public class ReconfigurationTimeCalculator : IReconfigurationTimeCalculator
{
    public IOrdersReconfigurationTimeCalculator OrdersReconfigurationTimeCalculator { get; }

    public ReconfigurationTimeCalculator(IOrdersReconfigurationTimeCalculator ordersReconfigurationTimeCalculator)
    {
        OrdersReconfigurationTimeCalculator = ordersReconfigurationTimeCalculator ?? throw new ArgumentNullException(nameof(ordersReconfigurationTimeCalculator));
    }

    public double Calculate(ProductionLineQueue productionLineQueue)
    {
        var result = 0d;

        for (var i = 0; i < productionLineQueue.Orders.Count - 1; i++)
        {
            result += OrdersReconfigurationTimeCalculator.Calculate(productionLineQueue.ProductionLine,
                productionLineQueue.Orders.ElementAt(i),
                productionLineQueue.Orders.ElementAt(i + 1));
        }

        return result;
    }
}

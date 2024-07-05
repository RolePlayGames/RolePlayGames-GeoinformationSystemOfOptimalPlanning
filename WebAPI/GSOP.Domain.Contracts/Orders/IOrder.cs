using GSOP.Domain.Contracts.Orders.Models;

namespace GSOP.Domain.Contracts.Orders;

/// <summary>
/// Order domain model
/// </summary>
public interface IOrder
{
    OrderNumber Number { get; }

    CustomerID CustomerID { get; }

    FilmRecipeID FilmRecipeID { get; }

    OrderWidth Width { get; }

    OrderQuantityInRunningMeter QuantityInRunningMeter { get; }

    OrderFinishedGoods FinishedGoods { get; }

    OrderWaste Waste { get; }

    OrderRollsCount RollsCount { get; }

    OrderPlannedDate PlannedDate { get; }

    OrderPriceOverdue PriceOverdue { get; }
}

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

    Task SetNumber(OrderNumber number);

    Task SetCustomerID(CustomerID customerID);

    Task SetFilmRecipeID(FilmRecipeID filmRecipeID);

    void SetWidth(OrderWidth width);

    void SetQuantityInRunningMeter(OrderQuantityInRunningMeter quantityInRunningMeter);

    void SetFinishedGoods(OrderFinishedGoods finishedGoods);

    void SetWaste(OrderWaste waste);

    void SetRollsCount(OrderRollsCount rollsCount);

    void SetPlannedDate(OrderPlannedDate plannedDate);

    void SetPriceOverdue(OrderPriceOverdue priceOverdue);
}

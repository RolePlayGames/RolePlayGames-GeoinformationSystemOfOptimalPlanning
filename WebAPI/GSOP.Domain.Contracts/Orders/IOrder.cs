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

    /// <summary>
    /// Validates and updates number (should be unique)
    /// </summary>
    /// <param name="name">Film type article</param>
    Task SetNumber(OrderNumber number);

    /// <summary>
    /// Validates and updates customer ID (should exist)
    /// </summary>
    /// <param name="name">Film type article</param>
    Task SetCustomerID(CustomerID customerID);

    /// <summary>
    /// Validates and updates film recipe (should exist)
    /// </summary>
    /// <param name="name">Film type article</param>
    Task SetFilmRecipeID(FilmRecipeID filmRecipeID);

    /// <summary>
    /// Updates width
    /// </summary>
    /// <param name="width">Width</param>
    void SetWidth(OrderWidth width);

    /// <summary>
    /// Updates quantity in running meter
    /// </summary>
    /// <param name="quantityInRunningMeter">Quantity in running meter</param>
    void SetQuantityInRunningMeter(OrderQuantityInRunningMeter quantityInRunningMeter);

    /// <summary>
    /// Updates finished goods
    /// </summary>
    /// <param name="finishedGoods">Finished goods</param>
    void SetFinishedGoods(OrderFinishedGoods finishedGoods);

    /// <summary>
    /// Updates waste
    /// </summary>
    /// <param name="waste">Waste</param>
    void SetWaste(OrderWaste waste);

    /// <summary>
    /// Updates rolls count
    /// </summary>
    /// <param name="rollsCount">Rolls count</param>
    void SetRollsCount(OrderRollsCount rollsCount);

    /// <summary>
    /// Updates planned date
    /// </summary>
    /// <param name="plannedDate">Planned date</param>
    void SetPlannedDate(OrderPlannedDate plannedDate);

    /// <summary>
    /// Updates price overdue
    /// </summary>
    /// <param name="priceOverdue">Price overdue</param>
    void SetPriceOverdue(OrderPriceOverdue priceOverdue);
}

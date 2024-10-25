using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.Orders.Exceptions;
using GSOP.Domain.Contracts.Orders.Models;

namespace GSOP.Domain.Orders;

/// <inheritdoc/>
public class Order : IOrder
{
    private readonly IOrderRepository _orderRepository;

    public OrderNumber Number { get; protected set; }

    public CustomerID CustomerID { get; protected set; }

    public FilmRecipeID FilmRecipeID { get; protected set; }

    public OrderWidth Width { get; protected set; }

    public OrderQuantityInRunningMeter QuantityInRunningMeter { get; protected set; }

    public OrderFinishedGoods FinishedGoods { get; protected set; }

    public OrderWaste Waste { get; protected set; }

    public OrderRollsCount RollsCount { get; protected set; }

    public OrderPlannedDate PlannedDate { get; protected set; }

    public OrderPriceOverdue PriceOverdue { get; protected set; }

    public Order(
        OrderNumber number,
        CustomerID customerID,
        FilmRecipeID filmRecipeID,
        OrderWidth width,
        OrderQuantityInRunningMeter quantityInRunningMeter,
        OrderFinishedGoods finishedGoods,
        OrderWaste waste,
        OrderRollsCount rollsCount,
        OrderPlannedDate plannedDate,
        OrderPriceOverdue priceOverdue,
        IOrderRepository orderRepository)
    {
        Number = number;
        CustomerID = customerID;
        FilmRecipeID = filmRecipeID;
        Width = width;
        QuantityInRunningMeter = quantityInRunningMeter;
        FinishedGoods = finishedGoods;
        Waste = waste;
        RollsCount = rollsCount;
        PlannedDate = plannedDate;
        PriceOverdue = priceOverdue;
        _orderRepository = orderRepository;
    }

    public async Task SetNumber(OrderNumber number)
    {
        if (Number != number)
        {
            var isNumberExists = await _orderRepository.IsNumberExists(number);

            if (isNumberExists)
                throw new OrderNumberAlreadyExistsException(number);

            Number = number;
        }
    }

    public async Task SetCustomerID(CustomerID customerID)
    {
        if (CustomerID != customerID)
        {
            var isCustomerExists = await _orderRepository.IsCustomerExists(customerID);

            if (!isCustomerExists)
                throw new CustomerDoesNotExistsException(customerID);

            CustomerID = customerID;
        }
    }

    public async Task SetFilmRecipeID(FilmRecipeID filmRecipeID)
    {
        if (FilmRecipeID != filmRecipeID)
        {
            var isFilmRecipeExists = await _orderRepository.IsFilmRecipeExists(filmRecipeID);

            if (!isFilmRecipeExists)
                throw new FilmRecipeDoesNotExistsException(filmRecipeID);

            FilmRecipeID = filmRecipeID;
        }
    }

    public void SetWidth(OrderWidth width)
    {
        Width = width;
    }

    public void SetQuantityInRunningMeter(OrderQuantityInRunningMeter quantityInRunningMeter)
    {
        QuantityInRunningMeter = quantityInRunningMeter;
    }

    public void SetFinishedGoods(OrderFinishedGoods finishedGoods)
    {
        FinishedGoods = finishedGoods;
    }

    public void SetWaste(OrderWaste waste)
    {
        Waste = waste;
    }

    public void SetRollsCount(OrderRollsCount rollsCount)
    {
        RollsCount = rollsCount;
    }

    public void SetPlannedDate(OrderPlannedDate plannedDate)
    {
        PlannedDate = plannedDate;
    }

    public void SetPriceOverdue(OrderPriceOverdue priceOverdue)
    {
        PriceOverdue = priceOverdue;
    }
}

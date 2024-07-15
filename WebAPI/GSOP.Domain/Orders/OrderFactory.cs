using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.Orders.Exceptions;
using GSOP.Domain.Contracts.Orders.Models;

namespace GSOP.Domain.Orders;

/// <inheritdoc/>
public class OrderFactory : IOrderFactory
{
    private readonly IOrderRepository _orderRepository;

    public OrderFactory(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <inheritdoc/>
    public async Task<IOrder> Create(long id)
    {
        var orderID = new ID(id);

        var order = await _orderRepository.Get(orderID) ?? throw new OrderWasNotFoundException(orderID);

        var number = new OrderNumber(order.Number);
        var customerID = new CustomerID(order.CustomerID);
        var filmRecipeID = new FilmRecipeID(order.FilmRecipeID);
        var width = new OrderWidth(order.Width);
        var quantityInRunningMeter = new OrderQuantityInRunningMeter(order.QuantityInRunningMeter);
        var finishedGoods = new OrderFinishedGoods(order.FinishedGoods);
        var waste = new OrderWaste(order.Waste);
        var rollsCount = new OrderRollsCount(order.RollsCount);
        var plannedDate = new OrderPlannedDate(order.PlannedDate);
        var priceOverdue = new OrderPriceOverdue(order.PriceOverdue);

        return new Order(
            number,
            customerID,
            filmRecipeID,
            width,
            quantityInRunningMeter,
            finishedGoods,
            waste,
            rollsCount,
            plannedDate,
            priceOverdue,
            _orderRepository);
    }

    /// <inheritdoc/>
    public async Task<IOrder> Create(OrderDTO order)
    {
        var number = new OrderNumber(order.Number);

        var isNumberExsits = await _orderRepository.IsNumberExists(number);

        if (isNumberExsits)
            throw new OrderNumberAlreadyExistsException(number);

        var customerID = new CustomerID(order.CustomerID);

        var isCustomerExists = await _orderRepository.IsCustomerExists(customerID);

        if (!isCustomerExists)
            throw new CustomerDoesNotExistsException(customerID);

        var filmRecipeID = new FilmRecipeID(order.FilmRecipeID);

        var isFilmRecipeExists = await _orderRepository.IsFilmRecipeExists(filmRecipeID);

        if (!isFilmRecipeExists)
            throw new FilmRecipeDoesNotExistsException(filmRecipeID);

        var width = new OrderWidth(order.Width);
        var quantityInRunningMeter = new OrderQuantityInRunningMeter(order.QuantityInRunningMeter);
        var finishedGoods = new OrderFinishedGoods(order.FinishedGoods);
        var waste = new OrderWaste(order.Waste);
        var rollsCount = new OrderRollsCount(order.RollsCount);
        var plannedDate = new OrderPlannedDate(order.PlannedDate);
        var priceOverdue = new OrderPriceOverdue(order.PriceOverdue);

        return new Order(
            number,
            customerID,
            filmRecipeID,
            width,
            quantityInRunningMeter,
            finishedGoods,
            waste,
            rollsCount,
            plannedDate,
            priceOverdue,
            _orderRepository);
    }
}

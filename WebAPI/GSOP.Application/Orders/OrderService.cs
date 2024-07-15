using GSOP.Application.Contracts.Orders;
using GSOP.Domain.Contracts.Orders.Exceptions;
using GSOP.Domain.Contracts.Orders.Models;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts;

namespace GSOP.Application.Orders;

/// <inheritdoc/>
public class OrderService : IOrderService
{
    private readonly IOrderFactory _orderFactory;
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderFactory orderFactory, IOrderRepository orderRepository)
    {
        _orderFactory = orderFactory;
        _orderRepository = orderRepository;
    }

    /// <inheritdoc/>
    public async Task<long> CreateOrder(OrderDTO order)
    {
        var newFilmType = await _orderFactory.Create(order);

        return await _orderRepository.Create(newFilmType);
    }

    /// <inheritdoc/>
    public async Task DeleteOrder(long id)
    {
        var orderId = new ID(id);

        var isorderDeleted = await _orderRepository.Delete(orderId);

        if (!isorderDeleted)
            throw new OrderWasNotFoundException(orderId);
    }

    /// <inheritdoc/>
    public async Task<OrderDTO> GetOrder(long id)
    {
        var orderId = new ID(id);
        return await _orderRepository.Get(orderId) ?? throw new OrderWasNotFoundException(orderId);
    }

    /// <inheritdoc/>
    public Task<IReadOnlyCollection<OrderInfo>> GetOrdersInfo()
    {
        return _orderRepository.GetInfos();
    }

    /// <inheritdoc/>
    public async Task UpdateOrder(long id, OrderDTO order)
    {
        var orderId = new ID(id);
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

        var existingOrder = await _orderFactory.Create(id);

        await existingOrder.SetNumber(number);
        await existingOrder.SetCustomerID(customerID);
        await existingOrder.SetFilmRecipeID(filmRecipeID);
        existingOrder.SetWidth(width);
        existingOrder.SetQuantityInRunningMeter(quantityInRunningMeter);
        existingOrder.SetFinishedGoods(finishedGoods);
        existingOrder.SetWaste(waste);
        existingOrder.SetRollsCount(rollsCount);
        existingOrder.SetPlannedDate(plannedDate);
        existingOrder.SetPriceOverdue(priceOverdue);

        await _orderRepository.Update(orderId, existingOrder);
    }
}

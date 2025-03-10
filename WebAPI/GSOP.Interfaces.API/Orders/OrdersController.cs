using GSOP.Application.Contracts.Orders;
using GSOP.Domain.Contracts.Orders.Models;
using Microsoft.AspNetCore.Mvc;

namespace GSOP.Interfaces.API.Orders;

[ApiController]
[TypeFilter<OrdersExceptionFilter>]
[Route("api/orders")]
public class OrdersController
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IOrderService _orderService;

    public OrdersController(ILogger<OrdersController> logger, IOrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }

    [HttpPost]
    public Task<long> CreateOrder(OrderDTO order)
    {
        return _orderService.CreateOrder(order);
    }

    [HttpDelete]
    [Route("{id}")]
    public Task DeleteOrder(long id)
    {
        return _orderService.DeleteOrder(id);
    }

    [HttpGet]
    [Route("{id}")]
    public Task<OrderDTO> GetOrder(long id)
    {
        return _orderService.GetOrder(id);
    }

    [HttpGet]
    [Route("info")]
    public Task<IReadOnlyCollection<OrderInfo>> GetOrdersInfo()
    {
        return _orderService.GetOrdersInfo();
    }

    [HttpPost]
    [Route("{id}")]
    public Task UpdateOrder(long id, OrderDTO order)
    {
        return _orderService.UpdateOrder(id, order);
    }
}

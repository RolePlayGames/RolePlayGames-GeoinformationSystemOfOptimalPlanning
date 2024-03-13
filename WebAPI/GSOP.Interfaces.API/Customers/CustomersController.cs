using GSOP.Application.Contracts.Customers;
using GSOP.Domain.Contracts.Customers.Models;
using Microsoft.AspNetCore.Mvc;

namespace GSOP.Interfaces.API.Customers;

[ApiController]
[TypeFilter<CustomersExceptionFilter>]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ILogger<CustomersController> _logger;
    private readonly ICustomerService _customerService;

    public CustomersController(ILogger<CustomersController> logger, ICustomerService customerService)
    {
        _logger = logger;
        _customerService = customerService;
    }

    [HttpPost]
    public Task<long> CreateCustomer(CustomerDTO customer)
    {
        return _customerService.CreateCustomer(customer);
    }

    [HttpDelete]
    [Route("{id}")]
    public Task DeleteCustomer(long id)
    {
        return _customerService.DeleteCustomer(id);
    }

    [HttpGet]
    [Route("{id}")]
    public Task<CustomerDTO> GetCustomer(long id)
    {
        return _customerService.GetCustomer(id);
    }

    [HttpGet]
    [Route("info")]
    public Task<IReadOnlyCollection<CustomerInfo>> GetCustomersInfo()
    {
        return _customerService.GetCustomersInfo();
    }

    [HttpPost]
    [Route("{id}")]
    public Task UpdateCustomer(long id, CustomerDTO customer)
    {
        return _customerService.UpdateCustomer(id, customer);
    }
}
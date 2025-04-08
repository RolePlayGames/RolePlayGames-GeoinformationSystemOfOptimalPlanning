using GSOP.Application.Contracts.Customers;
using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.Customers;
using GSOP.Domain.Contracts.Customers.Exceptions;
using GSOP.Domain.Contracts.Customers.Models;
using GSOP.Domain.Contracts.Routes;

namespace GSOP.Application.Customers;

/// <inheritdoc/>
public class CustomerService : ICustomerService
{
    private readonly ICustomerFactory _customerFactory;
    private readonly ICustomerRepository _customerRepository;
    private readonly IRouteFactory _routeFactory;
    private readonly IRouteRepository _routeRepository;

    public CustomerService(ICustomerFactory customerFactory, ICustomerRepository customerRepository, IRouteFactory routeFactory, IRouteRepository routeRepository)
    {
        _customerFactory = customerFactory;
        _customerRepository = customerRepository;
        _routeFactory = routeFactory;
        _routeRepository = routeRepository;
    }

    /// <inheritdoc/>
    public async Task<long> CreateCustomer(CustomerDTO customer)
    {
        var newCustomer = await _customerFactory.CreateCustomer(customer);

        var customerId = await _customerRepository.CreateCustomer(newCustomer);

        var routes = await _routeFactory.CreateCustomerRoutes(new(customerId));

        foreach (var route in routes)
        {
            await _routeRepository.Create(route);
        }

        return customerId;
    }

    /// <inheritdoc/>
    public async Task DeleteCustomer(long id)
    {
        var customerId = new ID(id);

        var isCustomerDeleted = await _customerRepository.DeleteCustomer(customerId);

        if (!isCustomerDeleted)
            throw new CustomerWasNotFoundException(customerId);
    }

    /// <inheritdoc/>
    public async Task<CustomerDTO> GetCustomer(long id)
    {
        var customerId = new ID(id);
        var customer = await _customerRepository.GetCustomer(customerId);

        return customer is null ? throw new CustomerWasNotFoundException(customerId) : customer;
    }

    /// <inheritdoc/>
    public Task<IReadOnlyCollection<CustomerInfo>> GetCustomersInfo()
    {
        return _customerRepository.GetCustomersInfo();
    }

    /// <inheritdoc/>
    public async Task UpdateCustomer(long id, CustomerDTO customer)
    {
        var customerId = new ID(id);
        var customerName = new CustomerName(customer.Name);

        var existingCustomer = await _customerFactory.CreateCustomer(id);

        await existingCustomer.SetName(customerName);
        existingCustomer.SetCoordinates(customer.Coordinates is null ? null : new(new(customer.Coordinates.Latitude), new(customer.Coordinates.Longitude)));

        await _customerRepository.UpdateCustomer(customerId, existingCustomer);
    }
}

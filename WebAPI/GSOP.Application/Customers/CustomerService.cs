using GSOP.Application.Contracts.Customers;
using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.Customers;
using GSOP.Domain.Contracts.Customers.Exceptions;
using GSOP.Domain.Contracts.Customers.Models;

namespace GSOP.Application.Customers;

/// <inheritdoc/>
public class CustomerService : ICustomerService
{
    private readonly ICustomerFactory _customerFactory;
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerFactory customerFactory, ICustomerRepository customerRepository)
    {
        _customerFactory = customerFactory;
        _customerRepository = customerRepository;
    }

    /// <inheritdoc/>
    public async Task<long> CreateCustomer(CustomerDTO customer)
    {
        var newCustomer = await _customerFactory.CreateCustomer(customer);

        return await _customerRepository.CreateCustomer(newCustomer);
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

        if (customer is null)
            throw new CustomerWasNotFoundException(customerId);

        return customer;
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

        await _customerRepository.UpdateCustomer(customerId, existingCustomer);
    }
}

using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.Customers;
using GSOP.Domain.Contracts.Customers.Exceptions;
using GSOP.Domain.Contracts.Customers.Models;

namespace GSOP.Domain.Customers;

/// <inheritdoc/>
public class CustomerFactory : ICustomerFactory
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerFactory(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    /// <inheritdoc/>
    public async Task<ICustomer> CreateCustomer(long id)
    {
        var customerId = new ID(id);

        var customer = await _customerRepository.GetCustomer(customerId);

        if (customer is null)
            throw new CustomerWasNotFoundException(customerId);

        var name = new CustomerName(customer.Name);

        return new Customer(name, _customerRepository);
    }

    /// <inheritdoc/>
    public async Task<ICustomer> CreateCustomer(CustomerDTO customer)
    {
        var name = new CustomerName(customer.Name);

        var isCustomerNameExsits = await _customerRepository.IsCustomerNameExsits(name);

        if (isCustomerNameExsits)
            throw new CustomerNameAlreadyExistsException(name);

        return new Customer(name, _customerRepository);
    }
}

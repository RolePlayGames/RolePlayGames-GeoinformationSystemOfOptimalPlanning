﻿using GSOP.Domain.Contracts.Customers;
using GSOP.Domain.Contracts.Customers.Exceptions;
using GSOP.Domain.Contracts.Customers.Models;
using GSOP.Domain.Contracts.Locations;

namespace GSOP.Domain.Customers;

/// <inheritdoc/>
public class Customer : ICustomer
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerName Name { get; protected set; }

    public Coordinates? Coordinates { get; protected set; }

    public Customer(CustomerName name, Coordinates? coordinates, ICustomerRepository customerRepository)
    {
        Name = name;
        _customerRepository = customerRepository;
        Coordinates = coordinates;
    }

    public async Task SetName(CustomerName name)
    {
        if (Name != name)
        {
            var isNameExists = await _customerRepository.IsCustomerNameExsits(name);

            if (isNameExists)
                throw new CustomerNameAlreadyExistsException(name);

            Name = name;
        }
    }

    public void SetCoordinates(Coordinates? coordinates)
    {
        Coordinates = coordinates;
    }
}

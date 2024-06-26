﻿using GSOP.Domain.Contracts.Customers.Models;

namespace GSOP.Application.Contracts.Customers;

/// <summary>
/// Manages customer use cases
/// </summary>
public interface ICustomerService
{
    /// <summary>
    /// Creates new customer
    /// </summary>
    /// <param name="customer">Customer data</param>
    /// <returns>New customer ID</returns>
    Task<long> CreateCustomer(CustomerDTO customer);

    /// <summary>
    /// Deletes customer
    /// </summary>
    /// <param name="id">Customer ID</param>
    Task DeleteCustomer(long id);

    /// <summary>
    /// Gets customer by ID
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <returns>Customer</returns>
    Task<CustomerDTO> GetCustomer(long id);

    /// <summary>
    /// Returns customers small information
    /// </summary>
    Task<IReadOnlyCollection<CustomerInfo>> GetCustomersInfo();

    /// <summary>
    /// Updates customer
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <param name="customer">Customer data</param>
    Task UpdateCustomer(long id, CustomerDTO customer);
}

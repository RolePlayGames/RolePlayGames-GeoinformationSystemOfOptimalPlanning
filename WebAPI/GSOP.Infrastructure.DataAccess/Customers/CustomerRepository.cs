using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.Customers;
using GSOP.Domain.Contracts.Customers.Models;
using LinqToDB;

namespace GSOP.Infrastructure.DataAccess.Customers;

/// <inheritdoc/>
public class CustomerRepository : ICustomerRepository
{
    private readonly DatabaseConnection _connection;

    public CustomerRepository(DatabaseConnection connection)
    {
        _connection = connection;
    }

    /// <inheritdoc/>
    public Task<long> CreateCustomer(ICustomer customer)
    {
        return _connection.InsertWithInt64IdentityAsync(new CustomerPOCO { Name = customer.Name });
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteCustomer(ID id)
    {
        return await _connection.Customers
            .Where(x => x.ID == id)
            .DeleteAsync() == 1;
    }

    /// <inheritdoc/>
    public Task<CustomerDTO?> GetCustomer(ID id)
    {
        return _connection.Customers
            .Where(x => x.ID == id)
            .Select(x => new CustomerDTO { Name = x.Name })
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<CustomerInfo>> GetCustomersInfo()
    {
        return await _connection.Customers
            .Select(x => new CustomerInfo { ID = x.ID, Name = x.Name })
            .ToListAsync();
    }

    /// <inheritdoc/>
    public Task UpdateCustomer(ID id, ICustomer customer)
    {
        return _connection.Customers
            .Where(x => x.ID == id)
            .Set(x => x.Name, customer.Name)
            .UpdateAsync();
    }

    /// <inheritdoc/>
    public Task<bool> IsCustomerNameExsits(CustomerName customerName)
    {
        return _connection.Customers
            .Where(x => x.Name == customerName)
            .AnyAsync();
    }
}

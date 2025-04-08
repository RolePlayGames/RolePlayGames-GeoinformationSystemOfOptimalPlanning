using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.Customers;
using GSOP.Domain.Contracts.Customers.Models;
using GSOP.Domain.Contracts.Locations;
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
        return _connection.InsertWithInt64IdentityAsync(new CustomerPOCO { Name = customer.Name, Latitude = customer.Coordinates?.Latitude, Longitude = customer.Coordinates?.Longitude });
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
            .Select(x => new CustomerDTO { Name = x.Name, Coordinates = x.Latitude.HasValue && x.Longitude.HasValue ? new CoordinatesDTO { Latitude = x.Latitude.Value, Longitude = x.Longitude.Value } : null })
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
            .Set(x => x.Latitude, customer.Coordinates?.Latitude)
            .Set(x => x.Longitude, customer.Coordinates?.Longitude)
            .UpdateAsync();
    }

    /// <inheritdoc/>
    public Task<bool> IsCustomerNameExsits(CustomerName customerName)
    {
        return _connection.Customers
            .Where(x => x.Name == customerName)
            .AnyAsync();
    }

    /// <inheritdoc/>
    public Task<bool> IsCustomerExsits(ID id)
    {
        return _connection.Customers
            .Where(x => x.ID == id)
            .AnyAsync();
    }
}

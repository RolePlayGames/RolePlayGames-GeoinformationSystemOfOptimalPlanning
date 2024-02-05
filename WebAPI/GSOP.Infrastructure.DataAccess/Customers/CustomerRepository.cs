using GSOP.Domain.Contracts.Customers;

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
    public IReadOnlyCollection<CustomerInfo> GetCustomersInfo()
    {
        return _connection.Customers
            .Select(x => new CustomerInfo { ID = x.ID, Name = x.Name })
            .ToList();
    }
}

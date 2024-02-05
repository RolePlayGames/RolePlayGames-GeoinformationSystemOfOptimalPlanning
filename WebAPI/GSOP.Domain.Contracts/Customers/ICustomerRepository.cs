namespace GSOP.Domain.Contracts.Customers;

/// <summary>
/// Manages customer database logic
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// Gets customer short information
    /// </summary>
    /// <returns>Each customer info</returns>
    IReadOnlyCollection<CustomerInfo> GetCustomersInfo();
}

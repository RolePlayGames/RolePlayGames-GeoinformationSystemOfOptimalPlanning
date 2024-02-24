using GSOP.Domain.Contracts.Customers.Models;

namespace GSOP.Domain.Contracts.Customers.Exceptions
{
    /// <summary>
    /// Represents customer name is not unique
    /// </summary>
    public class CustomerNameAlreadyExistsException : Exception
    {
        public string CustomerName { get; }

        public CustomerNameAlreadyExistsException(CustomerName customerName) : base($"Customer name should be unique but already exists ({customerName})")
        {
            CustomerName = customerName;
        }
    }
}

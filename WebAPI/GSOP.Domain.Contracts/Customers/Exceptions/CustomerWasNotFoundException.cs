namespace GSOP.Domain.Contracts.Customers.Exceptions
{
    /// <summary>
    /// Represents customer was not found by ID
    /// </summary>
    public class CustomerWasNotFoundException : Exception
    {
        public long ID { get; }

        public CustomerWasNotFoundException(ID id) : base($"Customer was not found by ID ${id}")
        {
            ID = id;
        }
    }
}

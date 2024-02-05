namespace GSOP.Domain.Customers;

public class Customer
{
    required public ID ID { get; init; }

    required public CustomerName Name { get; init; }
}

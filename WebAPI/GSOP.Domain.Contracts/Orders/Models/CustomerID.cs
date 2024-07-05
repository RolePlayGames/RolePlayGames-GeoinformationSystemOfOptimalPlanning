namespace GSOP.Domain.Contracts.Orders.Models;

public record CustomerID : ID
{
    public CustomerID(long id) : base(id)
    {
    }
}

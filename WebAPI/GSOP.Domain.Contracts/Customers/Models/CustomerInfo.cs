namespace GSOP.Domain.Contracts.Customers.Models;

public record CustomerInfo
{
    public required long ID { get; init; }
    
    public required string Name { get; init; }
}

namespace GSOP.Domain.Contracts.Customers.Models;

public record CustomerDTO
{
    public required string Name { get; init; }

    public required CoordinatesDTO? Coordinates { get; init; }
}

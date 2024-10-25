namespace GSOP.Domain.Contracts.ProductionLines.Exceptions;

/// <summary>
/// Represents production line was not found by ID
/// </summary>
public class ProductionLineWasNotFoundException(ID id) : Exception($"Production line was not found by ID {id}")
{
    public long ID { get; } = id;
}

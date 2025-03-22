namespace GSOP.Domain.Contracts.Productions.Exceptions;

/// <summary>
/// Represents production was not found by ID
/// </summary>
public class ProductionWasNotFoundException : Exception
{
    public long ID { get; }

    public ProductionWasNotFoundException(ID id) : base($"Production was not found by ID {id}")
    {
        ID = id;
    }
}

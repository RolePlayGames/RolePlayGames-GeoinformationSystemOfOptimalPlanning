namespace GSOP.Domain.Contracts.Routes.Exceptions;

/// <summary>
/// Represents route was not found by ID
/// </summary>
public class RouteWasNotFoundException : Exception
{
    public long ID { get; }

    public RouteWasNotFoundException(ID id) : base($"Route was not found by ID {id}")
    {
        ID = id;
    }
}

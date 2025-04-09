namespace GSOP.Domain.Contracts.Productions.Exceptions;

/// <summary>
/// Represents route's production does not exists
/// </summary>
public class ProductionDoesNotExistsException : Exception
{
    public long ProductionID { get; }

    public ProductionDoesNotExistsException(ID productionID) : base($"Route's production {productionID} should exists but does not")
    {
        ProductionID = productionID;
    }
}

using GSOP.Domain.Contracts.Productions.Models;

namespace GSOP.Domain.Contracts.Productions.Exceptions;

/// <summary>
/// Represents production name is not unique
/// </summary>
public class ProductionNameAlreadyExistsException : Exception
{
    public string ProductionName { get; }

    public ProductionNameAlreadyExistsException(ProductionName productionName) : base($"Production name {productionName} should be unique but already exists")
    {
        ProductionName = productionName;
    }
}

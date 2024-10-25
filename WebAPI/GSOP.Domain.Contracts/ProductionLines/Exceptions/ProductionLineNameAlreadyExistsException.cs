using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.Contracts.ProductionLines.Exceptions;

/// <summary>
/// Represents production line name is not unique
/// </summary>
public class ProductionLineNameAlreadyExistsException(ProductionLineName productionLineName) : Exception($"Production line name {productionLineName} should be unique but already exists")
{
    public string ProductionLineName { get; } = productionLineName;
}

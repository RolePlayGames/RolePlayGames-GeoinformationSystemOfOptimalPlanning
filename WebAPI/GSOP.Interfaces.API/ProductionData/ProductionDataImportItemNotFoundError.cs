using GSOP.Application.Contracts.ProductionData;

namespace GSOP.Interfaces.API.ProductionData;

public record ProductionDataImportItemNotFoundError
{
    public string ErrorType { get; } = nameof(ProductionDataImportItemNotFoundError);

    public string ItemType { get; }

    public string Identifier { get; }

    public ProductionDataImportItemNotFoundError(ProductionDataImportItemNotFoundException ex)
    {
        ItemType = ex.ItemType.Name;
        Identifier = ex.Identifier;
    }
}

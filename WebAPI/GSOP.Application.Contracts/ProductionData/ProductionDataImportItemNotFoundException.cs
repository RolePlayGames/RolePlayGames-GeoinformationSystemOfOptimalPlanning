namespace GSOP.Application.Contracts.ProductionData;

public class ProductionDataImportItemNotFoundException : Exception
{
    public Type ItemType { get; }

    public string Identifier { get; }

    public ProductionDataImportItemNotFoundException(Type itemType, string identifier) : base($"Item of type {itemType} was not found by {identifier} on productiondata import")
    {
        ItemType = itemType;
        Identifier = identifier;
    }
}

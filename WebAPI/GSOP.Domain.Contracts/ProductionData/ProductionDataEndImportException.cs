namespace GSOP.Domain.Contracts.ProductionData;

public class ProductionDataEndImportException : Exception
{
    public ProductionDataEndImportException(Exception exception) : base($"There was an error on importing production data: {exception.Message}", exception)
    {

    }
}

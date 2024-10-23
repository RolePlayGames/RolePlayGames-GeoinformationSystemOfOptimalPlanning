namespace GSOP.Domain.Contracts.ProductionLines.Models;

public record ProductionLineID : ID
{
    public ProductionLineID(long id) : base(id)
    {
    }
}

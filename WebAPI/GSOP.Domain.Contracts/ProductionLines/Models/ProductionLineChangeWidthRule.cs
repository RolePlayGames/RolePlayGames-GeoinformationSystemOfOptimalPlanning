namespace GSOP.Domain.Contracts.ProductionLines.Models;

public record ProductionLineChangeWidthRule : ProductionLineChangeValueRule
{
    public ProductionLineChangeWidthRule(TimeSpan widthChangeTime, double widthChangeConsumption) : base(widthChangeTime, widthChangeConsumption)
    {

    }
}

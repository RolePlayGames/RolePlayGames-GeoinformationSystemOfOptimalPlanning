namespace GSOP.Domain.Contracts.ProductionLines.Models;

public record ProductionLineChangeThicknessRule : ProductionLineChangeValueRule
{
    public ProductionLineChangeThicknessRule(TimeSpan thicknessChangeTime, double thicknessChangeConsumption) : base(thicknessChangeTime, thicknessChangeConsumption)
    {

    }
}
namespace GSOP.Domain.Contracts.ProductionLines.Models;

public record ProductionLineChangeValueRule
{
    private const double _minChangeConsumption = 0;
    private static readonly TimeSpan _minChangeTime = TimeSpan.Zero;

    public TimeSpan ChangeTime { get; }

    public double ChangeConsumption { get; }

    public ProductionLineChangeValueRule(TimeSpan changeTime, double changeConsumption)
    {
        if (changeTime < _minChangeTime)
            throw new ArgumentOutOfRangeException(nameof(changeTime), $"Change time should be greater than {_minChangeTime}");

        if (changeConsumption < _minChangeConsumption)
            throw new ArgumentOutOfRangeException(nameof(changeConsumption), $"Change consumption should be greater than {_minChangeConsumption}");

        ChangeTime = changeTime;
        ChangeConsumption = changeConsumption;
    }
}

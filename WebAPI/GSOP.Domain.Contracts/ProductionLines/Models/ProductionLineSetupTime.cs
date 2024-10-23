namespace GSOP.Domain.Contracts.ProductionLines.Models;

public record ProductionLineSetupTime
{
    private static readonly TimeSpan _min = TimeSpan.Zero;

    private readonly TimeSpan _setupTime;

    public ProductionLineSetupTime(TimeSpan setupTime)
    {
        if (setupTime < _min)
            throw new ArgumentOutOfRangeException(nameof(setupTime), $"Setup time should be greater than {_min}");

        _setupTime = setupTime;
    }

    public static implicit operator TimeSpan(ProductionLineSetupTime setupTime) => setupTime._setupTime;

    public static explicit operator ProductionLineSetupTime(TimeSpan setupTime) => new(setupTime);
}

namespace GSOP.Domain.Contracts.ProductionLines.Models;

public record ProductionLineThicknessRange
{
    public const double Min = 0;

    public double MinThickness { get; }

    public double MaxThickness { get; }

    public ProductionLineThicknessRange(double minThickness, double maxThickness)
    {
        if (minThickness < Min)
            throw new ArgumentOutOfRangeException(nameof(minThickness), $"Min width should be greater than {Min}");

        if (maxThickness < minThickness)
            throw new ArgumentOutOfRangeException(nameof(maxThickness), $"Max width should be greater than min width");

        MinThickness = minThickness;
        MaxThickness = maxThickness;
    }

    public static implicit operator (double MinThickness, double MaxThickness)(ProductionLineThicknessRange thicknessRange) => (thicknessRange.MinThickness, thicknessRange.MaxThickness);

    public static explicit operator ProductionLineThicknessRange((double MinThickness, double MaxThickness) thicknessRange) => new(thicknessRange.MinThickness, thicknessRange.MaxThickness);
}

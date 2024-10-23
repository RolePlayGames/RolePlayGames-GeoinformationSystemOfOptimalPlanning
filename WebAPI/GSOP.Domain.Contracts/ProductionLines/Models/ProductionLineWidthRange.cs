namespace GSOP.Domain.Contracts.ProductionLines.Models;

public record ProductionLineWidthRange
{
    public const double Min = 0;

    public double MinWidth { get; }

    public double MaxWidth { get; }

    public ProductionLineWidthRange(double minWidth, double maxWidth)
    {
        if (minWidth < Min)
            throw new ArgumentOutOfRangeException(nameof(minWidth), $"Min width should be greater than {Min}");

        if (maxWidth < minWidth)
            throw new ArgumentOutOfRangeException(nameof(maxWidth), $"Max width should be greater than min width");

        MinWidth = minWidth;
        MaxWidth = maxWidth;
    }

    public static implicit operator (double MinWidth, double MaxWidth)(ProductionLineWidthRange widthRange) => (widthRange.MinWidth, widthRange.MaxWidth);

    public static explicit operator ProductionLineWidthRange((double MinWidth, double MaxWidth) widthRange) => new(widthRange.MinWidth, widthRange.MaxWidth);
}

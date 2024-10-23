namespace GSOP.Domain.Contracts.ProductionLines.Models;

public record ProductionLineName
{
    public const int MinLength = 3;
    public const int MaxLength = 20;

    private readonly string _name;

    public ProductionLineName(string name)
    {
        if (name.Length < MinLength || name.Length > MaxLength)
            throw new ArgumentOutOfRangeException(nameof(name), $"Name's length should be greater than {MinLength} and lesser than {MaxLength}");

        _name = name;
    }

    public static implicit operator string(ProductionLineName name) => name._name;

    public static explicit operator ProductionLineName(string name) => new(name);
}

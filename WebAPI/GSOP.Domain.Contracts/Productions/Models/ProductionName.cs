namespace GSOP.Domain.Contracts.Productions.Models;

public record ProductionName
{
    public const int MinLength = 1;
    public const int MaxLength = 500;

    private readonly string _name;

    public ProductionName(string name)
    {
        if (name.Length < MinLength || name.Length > MaxLength)
            throw new ArgumentOutOfRangeException(nameof(name), $"Production name's length should be greater than {MinLength} and lesser than {MaxLength}");

        _name = name;
    }

    public static implicit operator string(ProductionName name) => name._name;

    public static explicit operator ProductionName(string name) => new(name);
}

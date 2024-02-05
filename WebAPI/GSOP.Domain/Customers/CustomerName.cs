namespace GSOP.Domain.Customers;

public readonly record struct CustomerName
{
    public const int MinLength = 3;
    public const int MaxLength = 500;

    private readonly string _name;

    public CustomerName(string name)
    {
        if (name.Length < MinLength || name.Length > MaxLength)
            throw new ArgumentOutOfRangeException(nameof(name), $"Name's length should be greater than {MinLength} and lesser than {MaxLength}");

        _name = name;
    }

    public static implicit operator string(CustomerName name) => name._name;

    public static explicit operator CustomerName(string name) => new(name);
}

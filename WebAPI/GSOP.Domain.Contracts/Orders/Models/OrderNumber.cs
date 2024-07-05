namespace GSOP.Domain.Contracts.Orders.Models;

public record OrderNumber
{
    public const int MinLength = 1;
    public const int MaxLength = 20;

    private readonly string _number;

    public OrderNumber(string number)
    {
        if (number.Length < MinLength || number.Length > MaxLength)
            throw new ArgumentOutOfRangeException(nameof(number), $"Order number's length should be greater than {MinLength} and lesser than {MaxLength}");

        _number = number;
    }

    public static implicit operator string(OrderNumber number) => number._number;

    public static explicit operator OrderNumber(string number) => new(number);
}

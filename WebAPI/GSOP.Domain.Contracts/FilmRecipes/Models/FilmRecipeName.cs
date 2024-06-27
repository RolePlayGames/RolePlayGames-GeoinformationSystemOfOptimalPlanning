namespace GSOP.Domain.Contracts.FilmRecipes.Models;

public record FilmRecipeName
{
    public const int MinLength = 1;
    public const int MaxLength = 20;

    private readonly string _name;

    public FilmRecipeName(string name)
    {
        if (name.Length < MinLength || name.Length > MaxLength)
            throw new ArgumentOutOfRangeException(nameof(name), $"Name's length should be greater than {MinLength} and lesser than {MaxLength}");

        _name = name;
    }

    public static implicit operator string(FilmRecipeName name) => name._name;

    public static explicit operator FilmRecipeName(string name) => new(name);
}

namespace GSOP.Domain.Contracts.FilmTypes.Models;

public readonly record struct FilmTypeArticle
{
    public const int MinLength = 1;
    public const int MaxLength = 10;

    private readonly string _article;

    public FilmTypeArticle(string article)
    {
        if (article.Length < MinLength || article.Length > MaxLength)
            throw new ArgumentOutOfRangeException(nameof(article), $"Article's length should be greater than {MinLength} and lesser than {MaxLength}");

        _article = article;
    }

    public static implicit operator string(FilmTypeArticle article) => article._article;

    public static explicit operator FilmTypeArticle(string article) => new(article);
}

namespace GSOP.Domain.Contracts.FilmTypes.Models;

public record FilmTypeInfo
{
    public required long ID { get; init; }

    public required string Name { get; init; }
}

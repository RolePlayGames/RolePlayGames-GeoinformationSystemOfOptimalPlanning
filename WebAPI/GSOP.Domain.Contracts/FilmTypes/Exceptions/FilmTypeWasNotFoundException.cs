namespace GSOP.Domain.Contracts.FilmTypes.Exceptions;

/// <summary>
/// Represents film type was not found by ID
/// </summary>
public class FilmTypeWasNotFoundException : Exception
{
    public long ID { get; }

    public FilmTypeWasNotFoundException(ID id) : base($"Film type was not found by ID ${id}")
    {
        ID = id;
    }
}

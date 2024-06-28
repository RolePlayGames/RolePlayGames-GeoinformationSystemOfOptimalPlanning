namespace GSOP.Domain.Contracts;

public record ID
{
    private readonly long _id;

    public ID(long id)
    {
        if (id < 0)
            throw new ArgumentOutOfRangeException(nameof(id), "ID should be greater than or equal to 0");

        _id = id;
    }

    public static implicit operator long(ID id) => id._id;

    public static explicit operator ID(long id) => new(id);
}

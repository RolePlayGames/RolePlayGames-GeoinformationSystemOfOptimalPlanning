using LinqToDB.Mapping;

namespace GSOP.Infrastructure.DataAccess.FilmTypes;

[Table(Schema = DBConstants.Schema, Name = TableName)]
public class FilmTypePOCO
{
    public const string TableName = "film_types";

    [PrimaryKey, Identity]
    public long ID { get; init; }

    [Column]
    required public string Article { get; init; }
}

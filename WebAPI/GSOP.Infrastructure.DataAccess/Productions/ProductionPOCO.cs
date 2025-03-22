using LinqToDB.Mapping;

namespace GSOP.Infrastructure.DataAccess.Productions;

[Table(Schema = DBConstants.Schema, Name = TableName)]
public class ProductionPOCO
{
    public const string TableName = "productions";

    [PrimaryKey, Identity]
    public long ID { get; init; }

    [Column]
    public required string Name { get; init; }

    [Column]
    public required decimal? Latitude { get; init; }

    [Column]
    public required decimal? Longitude { get; init; }
}

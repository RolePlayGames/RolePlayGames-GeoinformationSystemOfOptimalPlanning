using LinqToDB.Mapping;

namespace GSOP.Infrastructure.DataAccess.Customers;

[Table(Schema = DBConstants.Schema, Name = TableName)]
public class CustomerPOCO
{
    public const string TableName = "customers";

    [PrimaryKey, Identity]
    public long ID { get; init; }

    [Column]
    public required string Name { get; init; }

    [Column]
    public required decimal? Latitude { get; init; }

    [Column]
    public required decimal? Longitude { get; init; }
}

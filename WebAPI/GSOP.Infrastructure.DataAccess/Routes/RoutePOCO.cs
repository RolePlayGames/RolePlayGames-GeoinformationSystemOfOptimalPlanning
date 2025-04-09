using LinqToDB.Mapping;

namespace GSOP.Infrastructure.DataAccess.Routes;

[Table(Schema = DBConstants.Schema, Name = TableName)]
public class RoutePOCO
{
    public const string TableName = "routes";

    [PrimaryKey, Identity]
    public long ID { get; init; }

    [Column]
    public required long ProductionID { get; init; }

    [Column]
    public required long CustomerID { get; init; }

    [Column]
    public required double Price { get; init; }

    [Column]
    public required TimeSpan DrivingTime { get; init; }
}

using LinqToDB.Mapping;

namespace GSOP.Infrastructure.DataAccess.Orders;

[Table(Schema = DBConstants.Schema, Name = TableName)]
public class OrderPOCO
{
    public const string TableName = "orders";

    [PrimaryKey, Identity]
    public long ID { get; init; }

    [Column]
    public required string Number { get; init; }

    [Column]
    public required long CustomerID { get; init; }

    [Column]
    public required long FilmRecipeID { get; init; }

    [Column]
    public required int Width { get; init; }

    [Column]
    public required int QuantityInRunningMeter { get; init; }

    [Column]
    public required double FinishedGoods { get; init; }

    [Column]
    public required double Waste { get; init; }

    [Column]
    public required int RollsCount { get; init; }

    [Column]
    public required DateTime? PlannedDate { get; init; }

    [Column]
    public required double PriceOverdue { get; init; }
}

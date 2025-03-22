using LinqToDB.Mapping;

namespace GSOP.Infrastructure.DataAccess.ProductionLines;

[Table(Schema = DBConstants.Schema, Name = TableName)]
public class ProductionLinePOCO
{
    public const string TableName = "production_lines";

    [PrimaryKey, Identity]
    public long ID { get; init; }

    [Column]
    public required string Name { get; init; }

    [Column]
    public required decimal HourCost { get; init; }

    [Column]
    public required double MaxProductionSpeed { get; init; }

    [Column]
    public required double WidthMin { get; init; }

    [Column]
    public required double WidthMax { get; init; }

    [Column]
    public required double ThicknessMin { get; init; }

    [Column]
    public required double ThicknessMax { get; init; }

    [Column]
    public required TimeSpan ThicknessChangeTime { get; init; }

    [Column]
    public required double ThicknessChangeConsumption { get; init; }

    [Column]
    public required TimeSpan WidthChangeTime { get; init; }

    [Column]
    public required double WidthChangeConsumption { get; init; }

    [Column]
    public required TimeSpan SetupTime { get; init; }

    [Column]
    public required long ProductionID { get; init; }
}

using LinqToDB.Mapping;

namespace GSOP.Infrastructure.DataAccess.ProductionLines.ProductionRules;

[Table(Schema = DBConstants.Schema, Name = TableName)]
public class NozzleChangeRulePOCO
{
    public const string TableName = "nozzle_change_rules";

    [PrimaryKey, Identity]
    public long ID { get; init; }

    [Column]
    public long ProductionLineID { get; init; }

    [Column]
    public required double NozzleTo { get; init; }

    [Column]
    public required TimeSpan ChangeTime { get; init; }

    [Column]
    public required double ChangeConsumption { get; init; }
}

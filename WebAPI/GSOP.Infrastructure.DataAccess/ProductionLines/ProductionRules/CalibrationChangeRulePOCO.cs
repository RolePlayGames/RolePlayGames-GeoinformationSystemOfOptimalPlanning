using LinqToDB.Mapping;

namespace GSOP.Infrastructure.DataAccess.ProductionLines.ProductionRules;

[Table(Schema = DBConstants.Schema, Name = TableName)]
public class CalibrationChangeRulePOCO
{
    public const string TableName = "calibration_change_rules";

    [PrimaryKey, Identity]
    public long ID { get; init; }

    [Column]
    public long ProductionLineID { get; init; }

    [Column]
    public required double CalibrationTo { get; init; }

    [Column]
    public required TimeSpan ChangeTime { get; init; }

    [Column]
    public required double ChangeConsumption { get; init; }
}

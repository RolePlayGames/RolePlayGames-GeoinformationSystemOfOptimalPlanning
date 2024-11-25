using LinqToDB.Mapping;

namespace GSOP.Infrastructure.DataAccess.ProductionLines.ProductionRules;

[Table(Schema = DBConstants.Schema, Name = TableName)]
public class FilmTypeChangeRulePOCO
{
    public const string TableName = "film_type_change_rules";

    [PrimaryKey, Identity]
    public long ID { get; init; }

    [Column]
    public long ProductionLineID { get; init; }

    [Column]
    public long FilmTypeFromID { get; init; }

    [Column]
    public long FilmTypeToID { get; init; }

    [Column]
    public required TimeSpan ChangeTime { get; init; }

    [Column]
    public required double ChangeConsumption { get; init; }
}

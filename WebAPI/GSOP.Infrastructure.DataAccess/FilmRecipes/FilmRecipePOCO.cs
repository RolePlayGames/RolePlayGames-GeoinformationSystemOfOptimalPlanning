using LinqToDB.Mapping;

namespace GSOP.Infrastructure.DataAccess.FilmRecipes;

[Table(Schema = DBConstants.Schema, Name = TableName)]
public class FilmRecipePOCO
{
    public const string TableName = "film_recipes";

    [PrimaryKey, Identity]
    public long ID { get; init; }

    [Column]
    public required string Name { get; init; }

    [Column]
    public required long FilmTypeID { get; init; }

    [Column]
    public required double Thickness { get; init; }

    [Column]
    public required double ProductionSpeed { get; init; }

    [Column]
    public required double MaterialCost { get; init; }

    [Column]
    public required double Nozzle { get; init; }

    [Column]
    public required double Calibration { get; init; }

    [Column]
    public required double CoolingLip { get; init; }
}

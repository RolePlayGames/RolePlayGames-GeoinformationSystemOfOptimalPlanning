using FluentMigrator;
using GSOP.Infrastructure.DataAccess.FilmRecipes;
using GSOP.Infrastructure.DataAccess.FilmTypes;

namespace GSOP.Infrastructure.DataAccess.Migrations;

[Migration(27062024232417)]
public class _27062024232417_FilmRecipeCreated : Migration
{
    public override void Up()
    {
        Create.Table(FilmRecipePOCO.TableName)
            .InSchema(DBConstants.Schema)
            .WithColumn(nameof(FilmRecipePOCO.ID)).AsInt64().PrimaryKey().Identity()
            .WithColumn(nameof(FilmRecipePOCO.Name)).AsString(20).NotNullable().Unique()
            .WithColumn(nameof(FilmRecipePOCO.FilmTypeID)).AsInt64().NotNullable()
            .WithColumn(nameof(FilmRecipePOCO.Thickness)).AsDouble().NotNullable()
            .WithColumn(nameof(FilmRecipePOCO.ProductionSpeed)).AsDouble().NotNullable()
            .WithColumn(nameof(FilmRecipePOCO.MaterialCost)).AsDouble().NotNullable()
            .WithColumn(nameof(FilmRecipePOCO.Nozzle)).AsDouble().NotNullable()
            .WithColumn(nameof(FilmRecipePOCO.Calibration)).AsDouble().NotNullable()
            .WithColumn(nameof(FilmRecipePOCO.CoolingLip)).AsDouble().NotNullable();

        Create.ForeignKey()
            .FromTable(FilmRecipePOCO.TableName).InSchema(DBConstants.Schema).ForeignColumn(nameof(FilmRecipePOCO.FilmTypeID))
            .ToTable(FilmTypePOCO.TableName).InSchema(DBConstants.Schema).PrimaryColumn(nameof(FilmTypePOCO.ID));
    }

    public override void Down()
    {
        Delete.Table(FilmRecipePOCO.TableName).InSchema(DBConstants.Schema);
    }
}

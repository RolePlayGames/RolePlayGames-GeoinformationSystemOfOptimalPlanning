using FluentMigrator;
using GSOP.Infrastructure.DataAccess.FilmTypes;

namespace GSOP.Infrastructure.DataAccess.Migrations;

[Migration(10052024215700)]
public class _10052024215700_FilmTypesCreated : Migration
{
    public override void Up()
    {
        Create.Table(FilmTypePOCO.TableName)
            .InSchema(DBConstants.Schema)
            .WithColumn(nameof(FilmTypePOCO.ID)).AsInt64().PrimaryKey().Identity()
            .WithColumn(nameof(FilmTypePOCO.Article)).AsString(10).Unique();
    }

    public override void Down()
    {
        Delete.Table(FilmTypePOCO.TableName).InSchema(DBConstants.Schema);
    }
}
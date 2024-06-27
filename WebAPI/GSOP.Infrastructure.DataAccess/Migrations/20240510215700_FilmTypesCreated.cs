using FluentMigrator;
using GSOP.Infrastructure.DataAccess.FilmTypes;

namespace GSOP.Infrastructure.DataAccess.Migrations;

[Migration(20240510215700)]
public class _20240510215700_FilmTypesCreated : Migration
{
    public override void Up()
    {
        Create.Table(FilmTypePOCO.TableName)
            .InSchema(DBConstants.Schema)
            .WithColumn(nameof(FilmTypePOCO.ID)).AsInt64().PrimaryKey().Identity()
            .WithColumn(nameof(FilmTypePOCO.Article)).AsString(10).NotNullable().Unique();
    }

    public override void Down()
    {
        Delete.Table(FilmTypePOCO.TableName).InSchema(DBConstants.Schema);
    }
}
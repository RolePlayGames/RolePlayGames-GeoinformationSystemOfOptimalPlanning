using FluentMigrator;
using GSOP.Infrastructure.DataAccess.Productions;

namespace GSOP.Infrastructure.DataAccess.Migrations;

[Migration(20250320011800)]
public class _20250320011800_ProductionsCreated : Migration
{
    public override void Up()
    {
        Create.Table(ProductionPOCO.TableName)
            .InSchema(DBConstants.Schema)
            .WithColumn(nameof(ProductionPOCO.ID)).AsInt64().PrimaryKey().Identity()
            .WithColumn(nameof(ProductionPOCO.Name)).AsString(500).NotNullable().Unique();
    }

    public override void Down()
    {
        Delete.Table(ProductionPOCO.TableName).InSchema(DBConstants.Schema);
    }
}

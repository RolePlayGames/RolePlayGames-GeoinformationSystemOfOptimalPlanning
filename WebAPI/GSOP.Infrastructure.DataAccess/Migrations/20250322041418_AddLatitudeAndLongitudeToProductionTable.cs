using FluentMigrator;
using GSOP.Infrastructure.DataAccess.Productions;

namespace GSOP.Infrastructure.DataAccess.Migrations;

[Migration(20250322041418)]
public class _20250322041418_AddLatitudeAndLongitudeToProductionTable : Migration
{
    public override void Up()
    {
        Alter.Table(ProductionPOCO.TableName)
            .InSchema(DBConstants.Schema)
            .AddColumn(nameof(ProductionPOCO.Latitude)).AsDecimal().Nullable()
            .AddColumn(nameof(ProductionPOCO.Longitude)).AsDecimal().Nullable();
    }

    public override void Down()
    {
        Delete.Column(nameof(ProductionPOCO.Latitude)).FromTable(ProductionPOCO.TableName).InSchema(DBConstants.Schema);
        Delete.Column(nameof(ProductionPOCO.Longitude)).FromTable(ProductionPOCO.TableName).InSchema(DBConstants.Schema);
    }
}

using FluentMigrator;
using GSOP.Infrastructure.DataAccess.ProductionLines;

namespace GSOP.Infrastructure.DataAccess.Migrations;

[Migration(20240730221852)]
public class _20240730221852_ProductionLinessCreated : Migration
{
    public override void Up()
    {
        Create.Table(ProductionLinePOCO.TableName)
            .InSchema(DBConstants.Schema)
            .WithColumn(nameof(ProductionLinePOCO.ID)).AsInt64().PrimaryKey().Identity()
            .WithColumn(nameof(ProductionLinePOCO.Name)).AsString(20).NotNullable().Unique()
            .WithColumn(nameof(ProductionLinePOCO.HourCost)).AsDecimal().NotNullable()
            .WithColumn(nameof(ProductionLinePOCO.MaxProductionSpeed)).AsDouble().NotNullable()
            .WithColumn(nameof(ProductionLinePOCO.WidthMin)).AsDouble().NotNullable()
            .WithColumn(nameof(ProductionLinePOCO.WidthMax)).AsDouble().NotNullable()
            .WithColumn(nameof(ProductionLinePOCO.ThicknessMin)).AsDouble().NotNullable()
            .WithColumn(nameof(ProductionLinePOCO.ThicknessMax)).AsDouble().NotNullable()
            .WithColumn(nameof(ProductionLinePOCO.ThicknessChangeTime)).AsTime().NotNullable()
            .WithColumn(nameof(ProductionLinePOCO.ThicknessChangeConsumption)).AsDouble().NotNullable()
            .WithColumn(nameof(ProductionLinePOCO.WidthChangeTime)).AsTime().NotNullable()
            .WithColumn(nameof(ProductionLinePOCO.WidthChangeConsumption)).AsDouble().NotNullable()
            .WithColumn(nameof(ProductionLinePOCO.SetupTime)).AsTime().NotNullable()            ;
    }

    public override void Down()
    {
        Delete.Table(ProductionLinePOCO.TableName).InSchema(DBConstants.Schema);
    }
}

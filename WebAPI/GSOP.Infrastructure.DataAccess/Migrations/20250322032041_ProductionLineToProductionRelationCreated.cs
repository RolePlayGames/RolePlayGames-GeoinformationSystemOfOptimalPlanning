using FluentMigrator;
using GSOP.Infrastructure.DataAccess.ProductionLines;
using GSOP.Infrastructure.DataAccess.Productions;

namespace GSOP.Infrastructure.DataAccess.Migrations;

[Migration(20250322032041)]
public class _20250322032041_ProductionLineToProductionRelationCreated : Migration
{
    private const string _productionName = "Maria Soell GmbH Nidda";

    public override void Up()
    {
        Alter.Table(ProductionLinePOCO.TableName)
            .InSchema(DBConstants.Schema)
            .AddColumn(nameof(ProductionLinePOCO.ProductionID)).AsInt64().Nullable();

        Execute.Sql($"INSERT INTO {DBConstants.Schema}.{ProductionPOCO.TableName} (\"{nameof(ProductionPOCO.Name)}\") VALUES ('{_productionName}')");
        Execute.Sql($"UPDATE {DBConstants.Schema}.{ProductionLinePOCO.TableName} SET \"{nameof(ProductionLinePOCO.ProductionID)}\" = (SELECT \"{nameof(ProductionPOCO.ID)}\" FROM {DBConstants.Schema}.{ProductionPOCO.TableName} WHERE \"{nameof(ProductionPOCO.Name)}\" = '{_productionName}');");

        Alter.Column(nameof(ProductionLinePOCO.ProductionID))
            .OnTable(ProductionLinePOCO.TableName)
            .InSchema(DBConstants.Schema)
            .AsInt64()
            .NotNullable();

        Create.ForeignKey()
            .FromTable(ProductionLinePOCO.TableName).InSchema(DBConstants.Schema).ForeignColumn(nameof(ProductionLinePOCO.ProductionID))
            .ToTable(ProductionPOCO.TableName).InSchema(DBConstants.Schema).PrimaryColumn(nameof(ProductionPOCO.ID));
    }

    public override void Down()
    {
        Delete.Column(nameof(ProductionLinePOCO.ProductionID))
            .FromTable(ProductionLinePOCO.TableName).InSchema(DBConstants.Schema);
    }
}

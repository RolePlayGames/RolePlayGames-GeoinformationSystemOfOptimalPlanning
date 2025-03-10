using FluentMigrator;
using GSOP.Infrastructure.DataAccess.Orders;

namespace GSOP.Infrastructure.DataAccess.Migrations;

[Migration(20250309183300)]
public class _20250309183300_ChangeFinishedGoodsToDouble : Migration
{
    const string _tempColumnName = $"{nameof(OrderPOCO.FinishedGoods)}_temp";

    public override void Up()
    {

        Alter.Table(OrderPOCO.TableName)
            .InSchema(DBConstants.Schema)
            .AddColumn(_tempColumnName).AsDouble().Nullable();

        Execute.Sql($@"
            UPDATE {DBConstants.Schema}.""{OrderPOCO.TableName}""
            SET ""{_tempColumnName}"" = CAST(""{nameof(OrderPOCO.FinishedGoods)}"" AS DOUBLE PRECISION);
        ");

        Delete.Column(nameof(OrderPOCO.FinishedGoods)).FromTable(OrderPOCO.TableName).InSchema(DBConstants.Schema);
        Rename.Column(_tempColumnName).OnTable(OrderPOCO.TableName).InSchema(DBConstants.Schema).To(nameof(OrderPOCO.FinishedGoods));
        Alter.Column(nameof(OrderPOCO.FinishedGoods)).OnTable(OrderPOCO.TableName).InSchema(DBConstants.Schema).AsDouble().NotNullable();
    }

    public override void Down()
    {
        Alter.Table(OrderPOCO.TableName)
            .InSchema(DBConstants.Schema)
            .AddColumn(_tempColumnName).AsInt32().Nullable();

        Execute.Sql($@"
            UPDATE {DBConstants.Schema}.""{OrderPOCO.TableName}""
            SET ""{_tempColumnName}"" = CAST(""{nameof(OrderPOCO.FinishedGoods)}"" AS INTEGER);
        ");

        Delete.Column(nameof(OrderPOCO.FinishedGoods)).FromTable(OrderPOCO.TableName).InSchema(DBConstants.Schema);
        Rename.Column(_tempColumnName).OnTable(OrderPOCO.TableName).InSchema(DBConstants.Schema).To(nameof(OrderPOCO.FinishedGoods));
        Alter.Column(nameof(OrderPOCO.FinishedGoods)).OnTable(OrderPOCO.TableName).InSchema(DBConstants.Schema).AsInt32().NotNullable();
    }
}

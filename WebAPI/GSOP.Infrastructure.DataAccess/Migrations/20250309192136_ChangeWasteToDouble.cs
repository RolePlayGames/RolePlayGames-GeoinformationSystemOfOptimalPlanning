using FluentMigrator;
using GSOP.Infrastructure.DataAccess.Orders;

namespace GSOP.Infrastructure.DataAccess.Migrations;

public class _20250309192136_ChangeWasteToDouble : Migration
{
    private const string _tempColumnName = $"{nameof(OrderPOCO.Waste)}_temp";

    public override void Up()
    {
        Alter.Table(OrderPOCO.TableName)
            .InSchema(DBConstants.Schema)
            .AddColumn(_tempColumnName).AsDouble().Nullable();

        Execute.Sql($@"
                UPDATE {DBConstants.Schema}.""{OrderPOCO.TableName}""
                SET ""{_tempColumnName}"" = CAST(""{nameof(OrderPOCO.Waste)}"" AS DOUBLE PRECISION);
            ");

        Delete.Column(nameof(OrderPOCO.Waste)).FromTable(OrderPOCO.TableName).InSchema(DBConstants.Schema);
        Rename.Column(_tempColumnName).OnTable(OrderPOCO.TableName).InSchema(DBConstants.Schema).To(nameof(OrderPOCO.Waste));
        Alter.Column(nameof(OrderPOCO.Waste)).OnTable(OrderPOCO.TableName).InSchema(DBConstants.Schema).AsDouble().NotNullable();
    }

    public override void Down()
    {
        Alter.Table(OrderPOCO.TableName)
            .InSchema(DBConstants.Schema)
            .AddColumn(_tempColumnName).AsInt32().Nullable();

        Execute.Sql($@"
                UPDATE {DBConstants.Schema}.""{OrderPOCO.TableName}""
                SET ""{_tempColumnName}"" = CAST(""{nameof(OrderPOCO.Waste)}"" AS INTEGER);
            ");

        Delete.Column(nameof(OrderPOCO.Waste)).FromTable(OrderPOCO.TableName).InSchema(DBConstants.Schema);
        Rename.Column(_tempColumnName).OnTable(OrderPOCO.TableName).InSchema(DBConstants.Schema).To(nameof(OrderPOCO.Waste));
        Alter.Column(nameof(OrderPOCO.Waste)).OnTable(OrderPOCO.TableName).InSchema(DBConstants.Schema).AsInt32().NotNullable();
    }
}

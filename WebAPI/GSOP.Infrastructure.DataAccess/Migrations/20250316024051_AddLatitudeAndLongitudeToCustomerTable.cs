using FluentMigrator;
using GSOP.Infrastructure.DataAccess.Customers;

namespace GSOP.Infrastructure.DataAccess.Migrations;

[Migration(20250316024051)]
public class _20250316024051_AddLatitudeAndLongitudeToCustomerTable : Migration
{
    public override void Up()
    {
        Alter.Table(CustomerPOCO.TableName)
            .InSchema(DBConstants.Schema)
            .AddColumn(nameof(CustomerPOCO.Latitude)).AsDecimal().Nullable()
            .AddColumn(nameof(CustomerPOCO.Longitude)).AsDecimal().Nullable();
    }

    public override void Down()
    {
        Delete.Column(nameof(CustomerPOCO.Latitude)).FromTable(CustomerPOCO.TableName).InSchema(DBConstants.Schema);
        Delete.Column(nameof(CustomerPOCO.Longitude)).FromTable(CustomerPOCO.TableName).InSchema(DBConstants.Schema);
    }
}

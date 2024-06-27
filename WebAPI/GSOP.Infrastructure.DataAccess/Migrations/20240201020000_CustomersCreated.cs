using FluentMigrator;
using GSOP.Infrastructure.DataAccess.Customers;

namespace GSOP.Infrastructure.DataAccess.Migrations;

[Migration(20240201020000)]
public class _20240201020000_CustomersCreated : Migration
{
    public override void Up()
    {
        Create.Table(CustomerPOCO.TableName)
            .InSchema(DBConstants.Schema)
            .WithColumn(nameof(CustomerPOCO.ID)).AsInt64().PrimaryKey().Identity()
            .WithColumn(nameof(CustomerPOCO.Name)).AsString(500).NotNullable().Unique();
    }

    public override void Down()
    {
        Delete.Table(CustomerPOCO.TableName).InSchema(DBConstants.Schema);
    }
}

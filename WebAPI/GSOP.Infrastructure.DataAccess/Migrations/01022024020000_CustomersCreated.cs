using FluentMigrator;
using GSOP.Infrastructure.DataAccess.Customers;

namespace GSOP.Infrastructure.DataAccess.Migrations;

[Migration(01022024020000)]
public class _01022024020000_CustomersCreated : Migration
{
    public override void Up()
    {
        Create.Table(CustomerPOCO.TableName)
            .InSchema(DBConstants.Schema)
            .WithColumn(nameof(CustomerPOCO.ID)).AsInt64().PrimaryKey().Identity()
            .WithColumn(nameof(CustomerPOCO.Name)).AsString(500).Unique();
    }

    public override void Down()
    {
        Delete.Table(CustomerPOCO.TableName).InSchema(DBConstants.Schema);
    }
}

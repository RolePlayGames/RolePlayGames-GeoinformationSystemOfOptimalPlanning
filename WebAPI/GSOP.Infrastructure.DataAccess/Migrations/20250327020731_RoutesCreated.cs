using FluentMigrator;
using GSOP.Infrastructure.DataAccess.Customers;
using GSOP.Infrastructure.DataAccess.Productions;
using GSOP.Infrastructure.DataAccess.Routes;
using System.Data;

namespace GSOP.Infrastructure.DataAccess.Migrations;

[Migration(20250327020731)]
public class _20250327020731_RoutesCreated : Migration
{
    public override void Up()
    {
        Create.Table(RoutePOCO.TableName)
            .InSchema(DBConstants.Schema)
            .WithColumn(nameof(RoutePOCO.ID)).AsInt64().PrimaryKey().Identity()
            .WithColumn(nameof(RoutePOCO.CustomerID)).AsInt64().NotNullable()
            .WithColumn(nameof(RoutePOCO.ProductionID)).AsInt64().NotNullable()
            .WithColumn(nameof(RoutePOCO.Price)).AsDouble().NotNullable()
            .WithColumn(nameof(RoutePOCO.DrivingTime)).AsTime().NotNullable();

        Create.ForeignKey()
            .FromTable(RoutePOCO.TableName).InSchema(DBConstants.Schema).ForeignColumn(nameof(RoutePOCO.ProductionID))
            .ToTable(ProductionPOCO.TableName).InSchema(DBConstants.Schema).PrimaryColumn(nameof(ProductionPOCO.ID))
            .OnDelete(Rule.Cascade);

        Create.ForeignKey()
            .FromTable(RoutePOCO.TableName).InSchema(DBConstants.Schema).ForeignColumn(nameof(RoutePOCO.CustomerID))
            .ToTable(CustomerPOCO.TableName).InSchema(DBConstants.Schema).PrimaryColumn(nameof(CustomerPOCO.ID))
            .OnDelete(Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table(RoutePOCO.TableName).InSchema(DBConstants.Schema);
    }
}

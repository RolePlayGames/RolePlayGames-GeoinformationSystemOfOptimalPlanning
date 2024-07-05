using FluentMigrator;
using GSOP.Infrastructure.DataAccess.Customers;
using GSOP.Infrastructure.DataAccess.FilmRecipes;
using GSOP.Infrastructure.DataAccess.Orders;

namespace GSOP.Infrastructure.DataAccess.Migrations;

[Migration(20240704001842)]
public class _20240704001842_OrdersCreated : Migration
{
    public override void Up()
    {
        Create.Table(OrderPOCO.TableName)
            .InSchema(DBConstants.Schema)
            .WithColumn(nameof(OrderPOCO.ID)).AsInt64().PrimaryKey().Identity()
            .WithColumn(nameof(OrderPOCO.Number)).AsString(20).NotNullable().Unique()
            .WithColumn(nameof(OrderPOCO.CustomerID)).AsInt64().NotNullable()
            .WithColumn(nameof(OrderPOCO.FilmRecipeID)).AsInt64().NotNullable()
            .WithColumn(nameof(OrderPOCO.Width)).AsInt32().NotNullable()
            .WithColumn(nameof(OrderPOCO.QuantityInRunningMeter)).AsInt32().NotNullable()
            .WithColumn(nameof(OrderPOCO.FinishedGoods)).AsInt32().NotNullable()
            .WithColumn(nameof(OrderPOCO.Waste)).AsInt32().NotNullable()
            .WithColumn(nameof(OrderPOCO.RollsCount)).AsInt32().NotNullable()
            .WithColumn(nameof(OrderPOCO.PlannedDate)).AsDateTime().NotNullable()
            .WithColumn(nameof(OrderPOCO.PriceOverdue)).AsDouble().NotNullable();

        Create.ForeignKey()
            .FromTable(OrderPOCO.TableName).InSchema(DBConstants.Schema).ForeignColumn(nameof(OrderPOCO.CustomerID))
            .ToTable(CustomerPOCO.TableName).InSchema(DBConstants.Schema).PrimaryColumn(nameof(CustomerPOCO.ID));

        Create.ForeignKey()
            .FromTable(OrderPOCO.TableName).InSchema(DBConstants.Schema).ForeignColumn(nameof(OrderPOCO.FilmRecipeID))
            .ToTable(FilmRecipePOCO.TableName).InSchema(DBConstants.Schema).PrimaryColumn(nameof(FilmRecipePOCO.ID));
    }

    public override void Down()
    {
        Delete.Table(OrderPOCO.TableName).InSchema(DBConstants.Schema);
    }
}

using FluentMigrator;
using GSOP.Infrastructure.DataAccess.Customers;
using GSOP.Infrastructure.DataAccess.Productions;
using GSOP.Infrastructure.DataAccess.Routes;

namespace GSOP.Infrastructure.DataAccess.Migrations;

[Migration(20250408152931)]
public class _20250408152931_RoutesDataCreated : Migration
{
    public override void Up()
    {
        var sql = $@"
                INSERT INTO ""{DBConstants.Schema}"".""{RoutePOCO.TableName}"" (""{nameof(RoutePOCO.ProductionID)}"", ""{nameof(RoutePOCO.CustomerID)}"", ""{nameof(RoutePOCO.Price)}"", ""{nameof(RoutePOCO.DrivingTime)}"")
                SELECT
                    p.""ID""    AS ""{nameof(RoutePOCO.ProductionID)}"",
                    c.""ID""    AS ""{nameof(RoutePOCO.CustomerID)}"",
                    0.0         AS ""{nameof(RoutePOCO.Price)}"",
                    '00:00:00'  AS ""{nameof(RoutePOCO.DrivingTime)}""
                FROM ""{DBConstants.Schema}"".""{ProductionPOCO.TableName}"" p
                CROSS JOIN ""{DBConstants.Schema}"".""{CustomerPOCO.TableName}"" c;
            ";

        Execute.Sql(sql);
    }

    public override void Down()
    {
        Execute.Sql($@"
                DELETE FROM ""{DBConstants.Schema}"".""{RoutePOCO.TableName}"";
            ");
    }
}

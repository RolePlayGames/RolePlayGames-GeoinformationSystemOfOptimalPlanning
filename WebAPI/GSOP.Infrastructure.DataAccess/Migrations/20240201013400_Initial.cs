using FluentMigrator;

namespace GSOP.Infrastructure.DataAccess.Migrations;

[Migration(20240201013400)]
public class _20240201013400_Initial : Migration
{
    public override void Up()
    {
        Create.Schema(DBConstants.Schema);
    }

    public override void Down()
    {
        Delete.Schema(DBConstants.Schema);
    }
}

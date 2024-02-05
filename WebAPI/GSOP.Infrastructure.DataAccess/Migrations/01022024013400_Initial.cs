using FluentMigrator;

namespace GSOP.Infrastructure.DataAccess.Migrations;

[Migration(01022024013400)]
public class _01022024013400_Initial : Migration
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

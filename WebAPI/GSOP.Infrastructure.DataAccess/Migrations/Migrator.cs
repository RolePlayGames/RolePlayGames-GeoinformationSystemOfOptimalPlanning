using FluentMigrator.Runner;
using GSOP.Infrastructure.DataAccess.Contracts.Migrations;

namespace GSOP.Infrastructure.DataAccess.Migrations;

/// <inheritdoc/>
public class Migrator : IMigrator
{
    private readonly IMigrationRunner _migrationRunner;

    public Migrator(IMigrationRunner migrationRunner)
    {
        _migrationRunner = migrationRunner;
    }

    /// <inheritdoc/>
    public Task Migrate()
    {
        return Task.Run(() => _migrationRunner.MigrateUp());
    }
}

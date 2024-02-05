namespace GSOP.Infrastructure.DataAccess.Contracts.Migrations;

/// <summary>
/// Manages database migrations
/// </summary>
public interface IMigrator
{
    /// <summary>
    /// Migrates database
    /// </summary>
    Task Migrate();
}

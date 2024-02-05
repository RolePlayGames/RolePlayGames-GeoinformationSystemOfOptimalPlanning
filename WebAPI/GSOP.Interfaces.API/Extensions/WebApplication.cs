using GSOP.Infrastructure.DataAccess.Contracts.Migrations;

namespace GSOP.Interfaces.API.Extensions;

internal static class WebApplicationExtensions
{
    /// <summary>
    /// Migrates database
    /// </summary>
    /// <exception cref="InvalidOperationException">If cannot resolve migrator</exception>
    internal static Task MigrateDatabase(this WebApplication app)
    {
        var migrator = app.Services.CreateAsyncScope().ServiceProvider.GetService<IMigrator>()
            ?? throw new InvalidOperationException($"Cannot get {nameof(IMigrator)} to run database migrations");

        return migrator.Migrate();
    }
}

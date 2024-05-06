using GSOP.Infrastructure.DataAccess.Contracts.Migrations;
using GSOP.Interfaces.API.Home.PathGetters;
using Microsoft.Extensions.FileProviders;

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

    /// <summary>
    /// Add static files from UI
    /// </summary>
    /// <exception cref="InvalidOperationException">When cannot resolve IPathGetter or IDirectory</exception>
    internal static WebApplication UseUiStaticFiles(this WebApplication app)
    {
        var pathGetter = app.Services.GetService<IPathGetter>() ?? throw new InvalidOperationException("Cannot get IPathGetter service");
        var directory = app.Services.GetService<IDirectory>() ?? throw new InvalidOperationException("Cannot found IDirectory service");

        var path = pathGetter.UiStaticFilesFolderPath;

        directory.CreateDirectory(path);

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(path),
        });

        return app;
    }
}

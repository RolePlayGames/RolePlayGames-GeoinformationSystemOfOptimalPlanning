using FluentMigrator.Runner;
using GSOP.Infrastructure.DataAccess.Connections;
using Microsoft.Extensions.DependencyInjection;

namespace GSOP.Infrastructure.DataAccess.Migrations;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers data access components
    /// </summary>
    public static IServiceCollection AddMigratorConnection(this IServiceCollection serviceCollection)
        => serviceCollection
        .AddFluentMigratorCore()
        .ConfigureRunner(builder =>
        {
            var connectionsStringProvider = builder.Services.BuildServiceProvider().GetService<IConnectionStringProvider>()
                ?? throw new InvalidOperationException($"Cannot get {nameof(IConnectionStringProvider)} for fluent migrator setup");

            builder
                .AddPostgres()
                .WithGlobalConnectionString(connectionsStringProvider.DdlConnectionString)
                .ScanIn(typeof(_01022024013400_Initial).Assembly).For.Migrations();
        })
        .AddLogging(lb => lb.AddFluentMigratorConsole());
}

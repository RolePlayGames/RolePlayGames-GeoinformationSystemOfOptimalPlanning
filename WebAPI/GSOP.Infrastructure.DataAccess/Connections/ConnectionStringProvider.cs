using Microsoft.Extensions.Configuration;

namespace GSOP.Infrastructure.DataAccess.Connections;

/// <inheritdoc/>
public class ConnectionStringProvider : IConnectionStringProvider
{
    private const string _ddlConnectionStringPath = "DatabaseConnections:DdlConnection";
    private const string _dmlConnectionStringPath = "DatabaseConnections:DmlConnection";

    /// <inheritdoc/>
    public string DdlConnectionString => _configuration[_ddlConnectionStringPath] ?? throw new InvalidOperationException($"Cannot get {_ddlConnectionStringPath} configuration parameter");

    /// <inheritdoc/>
    public string DmlConnectionString => _configuration[_dmlConnectionStringPath] ?? throw new InvalidOperationException($"Cannot get {_dmlConnectionStringPath} configuration parameter");

    private readonly IConfiguration _configuration;

    public ConnectionStringProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }
}

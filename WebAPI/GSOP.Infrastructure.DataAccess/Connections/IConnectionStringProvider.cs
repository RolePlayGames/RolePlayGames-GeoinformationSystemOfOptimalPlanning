namespace GSOP.Infrastructure.DataAccess.Connections;

/// <summary>
/// Provide connection strings to database
/// </summary>
public interface IConnectionStringProvider
{
    /// <summary>
    /// DDL connection string
    /// </summary>
    string DdlConnectionString { get; }

    /// <summary>
    /// DML connection string
    /// </summary>
    string DmlConnectionString { get; }
}

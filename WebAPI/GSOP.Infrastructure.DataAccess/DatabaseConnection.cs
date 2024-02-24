using GSOP.Infrastructure.DataAccess.Customers;
using LinqToDB;

namespace GSOP.Infrastructure.DataAccess;

public class DatabaseConnection : LinqToDB.Data.DataConnection
{
    public virtual ITable<CustomerPOCO> Customers => this.GetTable<CustomerPOCO>();

    public DatabaseConnection(DataOptions<DatabaseConnection> options) : base(options.Options)
    {

    }
}

using GSOP.Infrastructure.DataAccess.Customers;
using LinqToDB;

namespace GSOP.Infrastructure.DataAccess;

public class DatabaseConnection : LinqToDB.Data.DataConnection
{
    public ITable<CustomerPOCO> Customers => this.GetTable<CustomerPOCO>();
}

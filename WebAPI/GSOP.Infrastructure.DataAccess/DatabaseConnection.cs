using GSOP.Infrastructure.DataAccess.Customers;
using GSOP.Infrastructure.DataAccess.FilmRecipes;
using GSOP.Infrastructure.DataAccess.FilmTypes;
using GSOP.Infrastructure.DataAccess.Orders;
using GSOP.Infrastructure.DataAccess.ProductionLines;
using LinqToDB;

namespace GSOP.Infrastructure.DataAccess;

public class DatabaseConnection : LinqToDB.Data.DataConnection
{
    public virtual ITable<CustomerPOCO> Customers => this.GetTable<CustomerPOCO>();

    public virtual ITable<FilmTypePOCO> FilmTypes => this.GetTable<FilmTypePOCO>();

    public virtual ITable<FilmRecipePOCO> FilmRecipes => this.GetTable<FilmRecipePOCO>();

    public virtual ITable<OrderPOCO> Orders => this.GetTable<OrderPOCO>();

    public virtual ITable<ProductionLinePOCO> ProductionLines => this.GetTable<ProductionLinePOCO>();

    public DatabaseConnection(DataOptions<DatabaseConnection> options) : base(options.Options)
    {

    }
}

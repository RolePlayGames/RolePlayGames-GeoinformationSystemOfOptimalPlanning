using GSOP.Infrastructure.DataAccess.Customers;
using GSOP.Infrastructure.DataAccess.FilmRecipes;
using GSOP.Infrastructure.DataAccess.FilmTypes;
using GSOP.Infrastructure.DataAccess.Orders;
using GSOP.Infrastructure.DataAccess.ProductionLines;
using GSOP.Infrastructure.DataAccess.ProductionLines.ProductionRules;
using GSOP.Infrastructure.DataAccess.Productions;
using GSOP.Infrastructure.DataAccess.Routes;
using LinqToDB;

namespace GSOP.Infrastructure.DataAccess;

public class DatabaseConnection : LinqToDB.Data.DataConnection
{
    public virtual ITable<CalibrationChangeRulePOCO> CalibrationChangeRules => this.GetTable<CalibrationChangeRulePOCO>();

    public virtual ITable<CoolingLipChangeRulePOCO> CoolingLipChangeRules => this.GetTable<CoolingLipChangeRulePOCO>();

    public virtual ITable<CustomerPOCO> Customers => this.GetTable<CustomerPOCO>();

    public virtual ITable<FilmTypeChangeRulePOCO> FilmTypeChangeRules => this.GetTable<FilmTypeChangeRulePOCO>();

    public virtual ITable<FilmTypePOCO> FilmTypes => this.GetTable<FilmTypePOCO>();

    public virtual ITable<FilmRecipePOCO> FilmRecipes => this.GetTable<FilmRecipePOCO>();

    public virtual ITable<NozzleChangeRulePOCO> NozzleChangeRules => this.GetTable<NozzleChangeRulePOCO>();

    public virtual ITable<OrderPOCO> Orders => this.GetTable<OrderPOCO>();

    public virtual ITable<ProductionPOCO> Productions => this.GetTable<ProductionPOCO>();

    public virtual ITable<ProductionLinePOCO> ProductionLines => this.GetTable<ProductionLinePOCO>();

    public virtual ITable<RoutePOCO> Routes => this.GetTable<RoutePOCO>();

    public DatabaseConnection(DataOptions<DatabaseConnection> options) : base(options.Options)
    {

    }
}

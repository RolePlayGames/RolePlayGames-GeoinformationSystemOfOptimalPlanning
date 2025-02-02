using GSOP.Application.Contracts.ProductionData.Models;
using GSOP.Application.Contracts.ProductionData.Models.ChangeRules;
using GSOP.Infrastructure.Excel.Contracts.ProductionData;
using OfficeOpenXml;

namespace GSOP.Infrastructure.Excel.ProductionData.Readers;

public class ProductionDataReader : IProductionDataReader
{
    private readonly IModelReader<FilmTypeModel> _filmTypeReader;
    private readonly IModelReader<FilmRecipeModel> _filmRecipeReader;
    private readonly IModelReader<CustomerModel> _customerReader;
    private readonly IModelReader<OrderModel> _orderReader;
    private readonly IModelReader<ProductionLineModel> _productionLineReader;
    private readonly IModelReader<CalibratoinChangeRuleModel> _calibrationChangeRuleReader;
    private readonly IModelReader<CoolingLipChangeRuleModel> _coolingLipChangeRuleReader;
    private readonly IModelReader<FilmTypeChangeRuleModel> _filmTypeChangeRuleReader;
    private readonly IModelReader<NozzleChangeRuleModel> _nozzleChangeRuleReader;

    public ProductionDataReader(
        IModelReader<FilmTypeModel> filmTypeReader,
        IModelReader<FilmRecipeModel> filmRecipeReader,
        IModelReader<CustomerModel> customerReader,
        IModelReader<OrderModel> orderReader,
        IModelReader<ProductionLineModel> productionLineReader,
        IModelReader<CalibratoinChangeRuleModel> calibrationChangeRuleReader,
        IModelReader<CoolingLipChangeRuleModel> coolingLipChangeRuleReader,
        IModelReader<FilmTypeChangeRuleModel> filmTypeChangeRuleReader,
        IModelReader<NozzleChangeRuleModel> nozzleChangeRuleReader)
    {
        _filmTypeReader = filmTypeReader;
        _filmRecipeReader = filmRecipeReader;
        _customerReader = customerReader;
        _orderReader = orderReader;
        _productionLineReader = productionLineReader;
        _calibrationChangeRuleReader = calibrationChangeRuleReader;
        _coolingLipChangeRuleReader = coolingLipChangeRuleReader;
        _filmTypeChangeRuleReader = filmTypeChangeRuleReader;
        _nozzleChangeRuleReader = nozzleChangeRuleReader;
    }

    public Application.Contracts.ProductionData.ProductionData ReadProductionData(Stream fileStream)
    {
        using var package = new ExcelPackage(fileStream);

        var filmTypes = _filmTypeReader.Read(package);
        var filmRecipes = _filmRecipeReader.Read(package);
        var customers = _customerReader.Read(package);
        var orders = _orderReader.Read(package);
        var productionLines = _productionLineReader.Read(package);
        var calibrationChangeRules = _calibrationChangeRuleReader.Read(package);
        var coolingLipChangeRules = _coolingLipChangeRuleReader.Read(package);
        var filmTypeChangeRules = _filmTypeChangeRuleReader.Read(package);
        var nozzleChangeRules = _nozzleChangeRuleReader.Read(package);

        return new()
        {
            FilmTypes = filmTypes,
            FilmRecipes = filmRecipes,
            Customers = customers,
            Orders = orders,
            ProductionLines = productionLines,
            CalibratoinChangeRules = calibrationChangeRules,
            CoolingLipChangeRules = coolingLipChangeRules,
            FilmTypeChangeRules = filmTypeChangeRules,
            NozzleChangeRules = nozzleChangeRules,
        };
    }
}

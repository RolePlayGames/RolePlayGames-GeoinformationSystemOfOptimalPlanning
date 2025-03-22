using OfficeOpenXml;
using GSOP.Application.Contracts.ProductionData.Models;
using GSOP.Application.Contracts.ProductionData.Models.ChangeRules;
using GSOP.Infrastructure.Excel.Contracts.ProductionData;

namespace GSOP.Infrastructure.Excel.ProductionData.Writers;

public class ProductionDataWriter : IProductionDataWriter
{
    private readonly IModelWriter<FilmTypeModel> _filmTypeWriter;
    private readonly IModelWriter<FilmRecipeModel> _filmRecipeWriter;
    private readonly IModelWriter<CustomerModel> _customerWriter;
    private readonly IModelWriter<OrderModel> _orderWriter;
    private readonly IModelWriter<ProductionLineModel> _productionLineWriter;
    private readonly IModelWriter<CalibratoinChangeRuleModel> _calibrationChangeRuleWriter;
    private readonly IModelWriter<CoolingLipChangeRuleModel> _coolingLipChangeRuleWriter;
    private readonly IModelWriter<FilmTypeChangeRuleModel> _filmTypeChangeRuleWriter;
    private readonly IModelWriter<NozzleChangeRuleModel> _nozzleChangeRuleWriter;
    private readonly IModelWriter<ProductionModel> _productionWriter;

    public ProductionDataWriter(
        IModelWriter<FilmTypeModel> filmTypeWriter,
        IModelWriter<FilmRecipeModel> filmRecipeWriter,
        IModelWriter<CustomerModel> customerWriter,
        IModelWriter<OrderModel> orderWriter,
        IModelWriter<ProductionLineModel> productionLineWriter,
        IModelWriter<CalibratoinChangeRuleModel> calibrationChangeRuleWriter,
        IModelWriter<CoolingLipChangeRuleModel> coolingLipChangeRuleWriter,
        IModelWriter<FilmTypeChangeRuleModel> filmTypeChangeRuleWriter,
        IModelWriter<NozzleChangeRuleModel> nozzleChangeRuleWriter,
        IModelWriter<ProductionModel> productionWriter)
    {
        _filmTypeWriter = filmTypeWriter;
        _filmRecipeWriter = filmRecipeWriter;
        _customerWriter = customerWriter;
        _orderWriter = orderWriter;
        _productionLineWriter = productionLineWriter;
        _calibrationChangeRuleWriter = calibrationChangeRuleWriter;
        _coolingLipChangeRuleWriter = coolingLipChangeRuleWriter;
        _filmTypeChangeRuleWriter = filmTypeChangeRuleWriter;
        _nozzleChangeRuleWriter = nozzleChangeRuleWriter;
        _productionWriter = productionWriter;
    }

    public async Task<Stream> Write(Application.Contracts.ProductionData.ProductionData productionData)
    {
        using var package = new ExcelPackage();

        _filmTypeWriter.Write(package, productionData.FilmTypes);
        _filmRecipeWriter.Write(package, productionData.FilmRecipes);
        _customerWriter.Write(package, productionData.Customers);
        _orderWriter.Write(package, productionData.Orders);
        _productionLineWriter.Write(package, productionData.ProductionLines);
        _calibrationChangeRuleWriter.Write(package, productionData.CalibratoinChangeRules);
        _coolingLipChangeRuleWriter.Write(package, productionData.CoolingLipChangeRules);
        _filmTypeChangeRuleWriter.Write(package, productionData.FilmTypeChangeRules);
        _nozzleChangeRuleWriter.Write(package, productionData.NozzleChangeRules);
        _productionWriter.Write(package, productionData.Productions);

        var stream = new MemoryStream();

        await package.SaveAsAsync(stream);
        stream.Seek(0, SeekOrigin.Begin);

        return stream;
    }
}

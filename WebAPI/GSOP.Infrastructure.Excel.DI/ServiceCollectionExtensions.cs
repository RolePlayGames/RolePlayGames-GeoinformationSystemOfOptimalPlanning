using GSOP.Application.Contracts.ProductionData.Models;
using GSOP.Application.Contracts.ProductionData.Models.ChangeRules;
using GSOP.Infrastructure.Excel.Contracts.ProductionData;
using GSOP.Infrastructure.Excel.ProductionData.Readers;
using GSOP.Infrastructure.Excel.ProductionData.Writers;
using Microsoft.Extensions.DependencyInjection;

namespace GSOP.Infrastructure.Excel.DI;

/// <summary>
/// Contains excel DI extensions
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers excel components
    /// </summary>
    public static IServiceCollection AddExcelComponents(this IServiceCollection serviceCollection)
        => serviceCollection
        .AddExcelReaders()
        .AddExcelWriters()
        ;

    private static IServiceCollection AddExcelReaders(this IServiceCollection serviceCollection)
        => serviceCollection
        .AddScoped<IModelReader<FilmTypeModel>, FilmTypeReader>()
        .AddScoped<IModelReader<FilmRecipeModel>, FilmRecipeReader>()
        .AddScoped<IModelReader<CustomerModel>, CustomerReader>()
        .AddScoped<IModelReader<OrderModel>, OrderReader>()
        .AddScoped<IModelReader<ProductionLineModel>, ProductionLineReader>()
        .AddScoped<IModelReader<CalibratoinChangeRuleModel>, CalibratoinChangeRuleReader>()
        .AddScoped<IModelReader<CoolingLipChangeRuleModel>, CoolingLipChangeRuleReader>()
        .AddScoped<IModelReader<FilmTypeChangeRuleModel>, FilmTypeChangeRuleReader>()
        .AddScoped<IModelReader<NozzleChangeRuleModel>, NozzleChangeRuleReader>()
        .AddScoped<IProductionDataReader, ProductionDataReader>()
        ;

    private static IServiceCollection AddExcelWriters(this IServiceCollection serviceCollection)
        => serviceCollection
        .AddScoped<IModelWriter<FilmTypeModel>, FilmTypeWriter>()
        .AddScoped<IModelWriter<FilmRecipeModel>, FilmRecipeWriter>()
        .AddScoped<IModelWriter<CustomerModel>, CustomerWriter>()
        .AddScoped<IModelWriter<OrderModel>, OrderWriter>()
        .AddScoped<IModelWriter<ProductionLineModel>, ProductionLineWriter>()
        .AddScoped<IModelWriter<CalibratoinChangeRuleModel>, CalibratoinChangeRuleWriter>()
        .AddScoped<IModelWriter<CoolingLipChangeRuleModel>, CoolingLipChangeRuleWriter>()
        .AddScoped<IModelWriter<FilmTypeChangeRuleModel>, FilmTypeChangeRuleWriter>()
        .AddScoped<IModelWriter<NozzleChangeRuleModel>, NozzleChangeRuleWriter>()
        .AddScoped<IProductionDataWriter, ProductionDataWriter>()
        ;
}

using GSOP.Application.Contracts.ProductionData.Models;
using GSOP.Application.Contracts.ProductionData.Models.ChangeRules;
using GSOP.Infrastructure.Excel.Contracts.ProductionData;
using GSOP.Infrastructure.Excel.ProductionData;
using GSOP.Infrastructure.Excel.ProductionData.Models;
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
}

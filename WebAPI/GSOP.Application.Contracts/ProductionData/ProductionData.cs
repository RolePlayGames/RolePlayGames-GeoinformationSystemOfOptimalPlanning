using GSOP.Application.Contracts.ProductionData.Models;
using GSOP.Application.Contracts.ProductionData.Models.ChangeRules;

namespace GSOP.Application.Contracts.ProductionData;

public record ProductionData
{
    public required IReadOnlyCollection<CustomerModel> Customers { get; init; }

    public required IReadOnlyCollection<FilmTypeModel> FilmTypes { get; init; }

    public required IReadOnlyCollection<FilmRecipeModel> FilmRecipes { get; init; }

    public required IReadOnlyCollection<OrderModel> Orders { get; init; }

    public required IReadOnlyCollection<ProductionLineModel> ProductionLines { get; init; }

    public required IReadOnlyCollection<CalibratoinChangeRuleModel> CalibratoinChangeRules { get; init; }

    public required IReadOnlyCollection<CoolingLipChangeRuleModel> CoolingLipChangeRules { get; init; }

    public required IReadOnlyCollection<FilmTypeChangeRuleModel> FilmTypeChangeRules { get; init; }

    public required IReadOnlyCollection<NozzleChangeRuleModel> NozzleChangeRules { get; init; }

    public required IReadOnlyCollection<ProductionModel> Productions { get; init; }
}

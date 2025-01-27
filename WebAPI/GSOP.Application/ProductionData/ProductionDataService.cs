using GSOP.Application.Contracts.Customers;
using GSOP.Application.Contracts.FilmRecipes;
using GSOP.Application.Contracts.FilmTypes;
using GSOP.Application.Contracts.Orders;
using GSOP.Application.Contracts.ProductionData;
using GSOP.Application.Contracts.ProductionLines;
using GSOP.Domain.Contracts.Customers.Models;
using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.FilmTypes.Models;
using GSOP.Domain.Contracts.Orders.Models;
using GSOP.Domain.Contracts.ProductionData;
using GSOP.Domain.Contracts.ProductionLines.Models;
using GSOP.Domain.Contracts.ProductionLines.ProductionRules;

namespace GSOP.Application.ProductionData;

public class ProductionDataService : IProductionDataService
{
    private readonly IProductionDataRepository _productionDataRepository;
    private readonly IFilmTypeSerivce _filmTypeSerivce;
    private readonly IFilmRecipeService _filmRecipeService;
    private readonly ICustomerService _customerService;
    private readonly IOrderService _orderService;
    private readonly IProductionLineService _productionLineService;

    public ProductionDataService(
        IProductionDataRepository productionDataRepository,
        IFilmTypeSerivce filmTypeSerivce,
        IFilmRecipeService filmRecipeService,
        ICustomerService customerService,
        IOrderService orderService,
        IProductionLineService productionLineService)
    {
        _productionDataRepository = productionDataRepository;
        _filmTypeSerivce = filmTypeSerivce;
        _filmRecipeService = filmRecipeService;
        _customerService = customerService;
        _orderService = orderService;
        _productionLineService = productionLineService;
    }

    public Task<Contracts.ProductionData.ProductionData> Export()
    {
        throw new NotImplementedException();
    }

    public async Task Import(Contracts.ProductionData.ProductionData data)
    {
        await using (await _productionDataRepository.StartImport())
        {
            await _productionDataRepository.DeleteProductionData();

            foreach (var dto in data.FilmTypes.Select(x => new FilmTypeDTO { Article = x.Article }))
            {
                await _filmTypeSerivce.CreateFilmType(dto);
            }

            var filmTypes = (await _filmTypeSerivce.GetFilmTypesInfo()).ToDictionary(x => x.Name, x => x.ID);

            foreach (var dto in data.FilmRecipes.Select(x => new FilmRecipeDTO
            {
                Name = x.Name,
                Nozzle = x.Nozzle,
                Calibration = x.Calibration,
                CoolingLip = x.CoolingLip,
                MaterialCost = x.MaterialCost,
                ProductionSpeed = x.ProductionSpeed,
                Thickness = x.Thickness,
                FilmTypeID = filmTypes.TryGetValue(x.FilmTypeArticle, out var value) ? value : throw new ProductionDataImportItemNotFoundException(typeof(FilmTypeDTO), x.FilmTypeArticle),
            }))
            {
                await _filmRecipeService.CreateFilmRecipe(dto);
            }

            foreach (var dto in data.Customers.Select(x => new CustomerDTO { Name = x.Name }))
            {
                await _customerService.CreateCustomer(dto);
            }

            var customers = (await _customerService.GetCustomersInfo()).ToDictionary(x => x.Name, x => x.ID);
            var filmRecipes = (await _filmRecipeService.GetFilmRecipesInfo()).ToDictionary(x => x.Name, x => x.ID);

            foreach (var dto in data.Orders.Select(x => new OrderDTO
            {
                CustomerID = customers.TryGetValue(x.CustomerName, out var customer) ? customer : throw new ProductionDataImportItemNotFoundException(typeof(CustomerDTO), x.CustomerName),
                FilmRecipeID = filmRecipes.TryGetValue(x.FilmRecipeName, out var recipe) ? recipe : throw new ProductionDataImportItemNotFoundException(typeof(FilmRecipeDTO), x.FilmRecipeName),
                FinishedGoods = x.FinishedGoods,
                PlannedDate = x.PlannedDate,
                Number = x.Number,
                PriceOverdue = x.PriceOverdue,
                RollsCount = x.RollsCount,
                QuantityInRunningMeter = x.QuantityInRunningMeter,
                Waste = x.Waste,
                Width = x.Width,
            }))
            {
                await _orderService.CreateOrder(dto);
            }

            foreach (var dto in data.ProductionLines.Select(x => new ProductionLineDTO
            {
                Name = x.Name,
                MaxProductionSpeed = x.MaxProductionSpeed,
                ThicknessMax = x.ThicknessMax,
                ThicknessMin = x.ThicknessMin,
                WidthMax = x.WidthMax,
                WidthMin = x.WidthMin,
                HourCost = x.HourCost,
                SetupTime = x.SetupTime,
                ThicknessChangeConsumption = x.ThicknessChangeConsumption,
                ThicknessChangeTime = x.ThicknessChangeTime,
                WidthChangeConsumption = x.WidthChangeConsumption,
                WidthChangeTime = x.WidthChangeTime,
                CalibratoinChangeRules = data.CalibratoinChangeRules
                    .Where(rule => rule.ProductionLineName == x.Name)
                    .Select(x => new CalibratoinChangeRuleDTO
                    {
                        CalibrationTo = x.CalibrationTo,
                        ChangeConsumption = x.ChangeConsumption,
                        ChangeTime = TimeSpan.FromMinutes(x.ChangeTimeMinutes),
                    }).ToList(),
                CoolingLipChangeRules = data.CoolingLipChangeRules
                    .Where(rule => rule.ProductionLineName == x.Name)
                    .Select(x => new CoolingLipChangeRuleDTO
                    {
                        CoolingLipTo = x.CoolingLipTo,
                        ChangeConsumption = x.ChangeConsumption,
                        ChangeTime = TimeSpan.FromMinutes(x.ChangeTimeMinutes),
                    }).ToList(),
                FilmTypeChangeRules = data.FilmTypeChangeRules
                    .Where(rule => rule.ProductionLineName == x.Name)
                    .Select(x => new FilmTypeChangeRuleDTO
                    {
                        FilmRecipeFromID = filmTypes.TryGetValue(x.FilmRecipeFromArticle, out var recipeFrom) ? recipeFrom : throw new ProductionDataImportItemNotFoundException(typeof(FilmTypeDTO), x.FilmRecipeFromArticle),
                        FilmRecipeToID = filmTypes.TryGetValue(x.FilmRecipeToArticle, out var recipeTo) ? recipeTo : throw new ProductionDataImportItemNotFoundException(typeof(FilmTypeDTO), x.FilmRecipeToArticle),
                        ChangeConsumption = x.ChangeConsumption,
                        ChangeTime = TimeSpan.FromMinutes(x.ChangeTimeMinutes),
                    }).ToList(),
                NozzleChangeRules = data.NozzleChangeRules
                    .Where(rule => rule.ProductionLineName == x.Name)
                    .Select(x => new NozzleChangeRuleDTO
                    {
                        NozzleTo = x.NozzleTo,
                        ChangeConsumption = x.ChangeConsumption,
                        ChangeTime = TimeSpan.FromMinutes(x.ChangeTimeMinutes),
                    }).ToList(),
            }))
            {
                await _productionLineService.CreateProductionLine(dto);
            }

            await _productionDataRepository.EndImport();
        }
    }
}

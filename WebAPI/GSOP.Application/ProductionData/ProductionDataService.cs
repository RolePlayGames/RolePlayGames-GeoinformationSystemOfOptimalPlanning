using GSOP.Application.Contracts.Customers;
using GSOP.Application.Contracts.FilmRecipes;
using GSOP.Application.Contracts.FilmTypes;
using GSOP.Application.Contracts.Orders;
using GSOP.Application.Contracts.ProductionData;
using GSOP.Application.Contracts.ProductionData.Models;
using GSOP.Application.Contracts.ProductionData.Models.ChangeRules;
using GSOP.Application.Contracts.ProductionLines;
using GSOP.Application.Contracts.Productions;
using GSOP.Application.Contracts.Routes;
using GSOP.Domain.Contracts.Customers.Models;
using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.FilmTypes.Models;
using GSOP.Domain.Contracts.Orders.Models;
using GSOP.Domain.Contracts.ProductionData;
using GSOP.Domain.Contracts.ProductionLines.Models;
using GSOP.Domain.Contracts.ProductionLines.ProductionRules;
using GSOP.Domain.Contracts.Productions.Models;
using GSOP.Domain.Contracts.Routes.Models;
using System.Collections.Frozen;

namespace GSOP.Application.ProductionData;

public class ProductionDataService : IProductionDataService
{
    private readonly IProductionDataRepository _productionDataRepository;
    private readonly IFilmTypeSerivce _filmTypeSerivce;
    private readonly IFilmRecipeService _filmRecipeService;
    private readonly ICustomerService _customerService;
    private readonly IOrderService _orderService;
    private readonly IProductionLineService _productionLineService;
    private readonly IProductionService _productionService;
    private readonly IRouteService _routeService;

    public ProductionDataService(
        IProductionDataRepository productionDataRepository,
        IFilmTypeSerivce filmTypeSerivce,
        IFilmRecipeService filmRecipeService,
        ICustomerService customerService,
        IOrderService orderService,
        IProductionLineService productionLineService,
        IProductionService productionService,
        IRouteService routeService)
    {
        _productionDataRepository = productionDataRepository;
        _filmTypeSerivce = filmTypeSerivce;
        _filmRecipeService = filmRecipeService;
        _customerService = customerService;
        _orderService = orderService;
        _productionLineService = productionLineService;
        _productionService = productionService;
        _routeService = routeService;
    }

    public async Task<Contracts.ProductionData.ProductionData> Export()
    {
        var filmTypesInfo = await _filmTypeSerivce.GetFilmTypesInfo();

        var filmTypes = new List<FilmTypeDTO>(filmTypesInfo.Count);

        foreach (var info in filmTypesInfo)
        {
            var filmType = await _filmTypeSerivce.GetFilmType(info.ID);
            filmTypes.Add(filmType);
        }

        var filmRecipesInfo = await _filmRecipeService.GetFilmRecipesInfo();

        var filmRecipes = new List<FilmRecipeDTO>(filmRecipesInfo.Count);

        foreach (var info in filmRecipesInfo)
        {
            var filmRecipe = await _filmRecipeService.GetFilmRecipe(info.ID);
            filmRecipes.Add(filmRecipe);
        }

        var customersInfo = await _customerService.GetCustomersInfo();

        var customers = new List<CustomerDTO>(customersInfo.Count);

        foreach (var info in customersInfo)
        {
            var customer = await _customerService.GetCustomer(info.ID);
            customers.Add(customer);
        }

        var ordersInfo = await _orderService.GetOrdersInfo();

        var orders = new List<OrderDTO>(ordersInfo.Count);

        foreach (var info in ordersInfo)
        {
            var order = await _orderService.GetOrder(info.ID);
            orders.Add(order);
        }

        var productionsInfo = await _productionService.GetProductionsInfo();

        var productions = new List<ProductionDTO>(productionsInfo.Count);

        foreach (var info in productionsInfo)
        {
            var productoin = await _productionService.GetProduction(info.ID);
            productions.Add(productoin);
        }

        var productionLinesInfo = await _productionLineService.GetProductionLinesInfo();

        var productionLines = new List<ProductionLineDTO>(productionLinesInfo.Count);

        foreach (var info in productionLinesInfo)
        {
            var productionLine = await _productionLineService.GetProductionLine(info.ID);
            productionLines.Add(productionLine);
        }

        var routesInfo = await _routeService.GetRoutesInfo();

        var routes = new List<RouteReadDTO>(customersInfo.Count * productionLinesInfo.Count);

        foreach (var info in routesInfo)
        {
            var route = await _routeService.GetRoute(info.ID);
            routes.Add(route);
        }

        var filmTypeArticleById = filmTypesInfo.ToDictionary(x => x.ID, x => x.Name);
        var customerNameById = customersInfo.ToDictionary(x => x.ID, x => x.Name);
        var filmRecipeNameById = filmRecipesInfo.ToDictionary(x => x.ID, x => x.Name);
        var productionNameById = productionsInfo.ToDictionary(x => x.ID, x => x.Name);

        return new Contracts.ProductionData.ProductionData()
        {
            FilmTypes = filmTypes.Select(x => new FilmTypeModel { Article = x.Article }).ToList(),
            FilmRecipes = filmRecipes.Select(x => new FilmRecipeModel
            {
                Calibration = x.Calibration,
                CoolingLip = x.CoolingLip,
                FilmTypeArticle = filmTypeArticleById.TryGetValue(x.FilmTypeID, out var filmTypeArticle) ? filmTypeArticle : throw new ProductionDataImportItemNotFoundException(typeof(FilmTypeDTO), x.FilmTypeID.ToString()),
                ProductionSpeed = x.ProductionSpeed,
                MaterialCost = x.MaterialCost,
                Name = x.Name,
                Nozzle = x.Nozzle,
                Thickness = x.Thickness,
            }).ToList(),
            Customers = customers.Select(x => new CustomerModel { Name = x.Name, Latitude = x.Coordinates?.Latitude, Longitude = x.Coordinates?.Longitude }).ToList(),
            Orders = orders.Select(x => new OrderModel
            {
                Number = x.Number,
                CustomerName = customerNameById.TryGetValue(x.CustomerID, out var customerName) ? customerName : throw new ProductionDataImportItemNotFoundException(typeof(CustomerDTO), x.CustomerID.ToString()),
                FilmRecipeName = filmRecipeNameById.TryGetValue(x.FilmRecipeID, out var filmRecipeName) ? filmRecipeName : throw new ProductionDataImportItemNotFoundException(typeof(FilmRecipeDTO), x.FilmRecipeID.ToString()),
                FinishedGoods = x.FinishedGoods,
                PlannedDate = x.PlannedDate,
                PriceOverdue = x.PriceOverdue,
                QuantityInRunningMeter = x.QuantityInRunningMeter,
                RollsCount = x.RollsCount,
                Waste = x.Waste,
                Width = x.Width,
            }).ToList(),
            ProductionLines = productionLines.Select(x => new ProductionLineModel
            {
                Name = x.Name,
                HourCost = x.HourCost,
                MaxProductionSpeed = x.MaxProductionSpeed,
                SetupTimeMinutes = (int)x.SetupTime.TotalMinutes,
                ThicknessChangeConsumption = x.ThicknessChangeConsumption,
                ThicknessChangeTimeMinutes = (int)x.ThicknessChangeTime.TotalMinutes,
                ThicknessMax = x.ThicknessMax,
                ThicknessMin = x.ThicknessMin,
                WidthChangeConsumption = x.WidthChangeConsumption,
                WidthChangeTimeMinutes = (int)x.WidthChangeTime.TotalMinutes,
                WidthMax = x.WidthMax,
                WidthMin = x.WidthMin,
                ProductionName = productionNameById.TryGetValue(x.ProductionID, out var productionName) ? productionName : throw new ProductionDataImportItemNotFoundException(typeof(ProductionDTO), x.ProductionID.ToString()),
            }).ToList(),
            CalibratoinChangeRules = productionLines.SelectMany(x => x.CalibratoinChangeRules.Select(rule => new CalibratoinChangeRuleModel
            {
                CalibrationTo = rule.CalibrationTo,
                ChangeConsumption = rule.ChangeConsumption,
                ChangeTimeMinutes = (int)rule.ChangeTime.TotalMinutes,
                ProductionLineName = x.Name,
            })).ToList(),
            CoolingLipChangeRules = productionLines.SelectMany(x => x.CoolingLipChangeRules.Select(rule => new CoolingLipChangeRuleModel
            {
                CoolingLipTo = rule.CoolingLipTo,
                ChangeConsumption = rule.ChangeConsumption,
                ChangeTimeMinutes = (int)rule.ChangeTime.TotalMinutes,
                ProductionLineName = x.Name,
            })).ToList(),
            FilmTypeChangeRules = productionLines.SelectMany(x => x.FilmTypeChangeRules.Select(rule => new FilmTypeChangeRuleModel
            {
                FilmRecipeFromArticle = filmTypeArticleById.TryGetValue(rule.FilmRecipeFromID, out var filmTypeFromArticle) ? filmTypeFromArticle : throw new ProductionDataImportItemNotFoundException(typeof(FilmTypeDTO), rule.FilmRecipeFromID.ToString()),
                FilmRecipeToArticle = filmTypeArticleById.TryGetValue(rule.FilmRecipeToID, out var filmTypeToArticle) ? filmTypeToArticle : throw new ProductionDataImportItemNotFoundException(typeof(FilmTypeDTO), rule.FilmRecipeToID.ToString()),
                ChangeConsumption = rule.ChangeConsumption,
                ChangeTimeMinutes = (int)rule.ChangeTime.TotalMinutes,
                ProductionLineName = x.Name,
            })).ToList(),
            NozzleChangeRules = productionLines.SelectMany(x => x.NozzleChangeRules.Select(rule => new NozzleChangeRuleModel
            {
                NozzleTo = rule.NozzleTo,
                ChangeConsumption = rule.ChangeConsumption,
                ChangeTimeMinutes = (int)rule.ChangeTime.TotalMinutes,
                ProductionLineName = x.Name,
            })).ToList(),
            Productions = productions.Select(x => new ProductionModel { Name = x.Name, Latitude = x.Coordinates?.Latitude, Longitude = x.Coordinates?.Longitude }).ToList(),
            Routes = routes.Select(x => new RouteModel { CustomerName = x.CustomerInfo.EntityName, ProductionName = x.ProductionInfo.EntityName, Price = x.Price, DrivingTimeMinutes = (int)x.DrivingTime.TotalMinutes }).ToList(),
        };
    }

    public async Task Import(Contracts.ProductionData.ProductionData data, bool shouldClearProductionData = false)
    {
        await using (await _productionDataRepository.StartImport())
        {
            if (shouldClearProductionData)
            {
                await _productionDataRepository.DeleteProductionData();
            }

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

            foreach (var dto in data.Customers.Select(x => new CustomerDTO { Name = x.Name, Coordinates = x.Latitude.HasValue && x.Longitude.HasValue ? new() { Latitude = x.Latitude.Value, Longitude = x.Longitude.Value } : null }))
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

            foreach (var dto in data.Productions.Select(x => new ProductionDTO
            {
                Name = x.Name,
                Coordinates = x.Latitude.HasValue && x.Longitude.HasValue
                ? new() { Latitude = x.Latitude.Value, Longitude = x.Longitude.Value }
                : null
            }))
            {
                await _productionService.CreateProduction(dto);
            }

            var productions = (await _productionService.GetProductionsInfo()).ToDictionary(x => x.Name, x => x.ID);

            foreach (var dto in data.ProductionLines.Select(x => new ProductionLineDTO
            {
                Name = x.Name,
                MaxProductionSpeed = x.MaxProductionSpeed,
                ThicknessMax = x.ThicknessMax,
                ThicknessMin = x.ThicknessMin,
                WidthMax = x.WidthMax,
                WidthMin = x.WidthMin,
                HourCost = x.HourCost,
                SetupTime = TimeSpan.FromMinutes(x.SetupTimeMinutes),
                ThicknessChangeConsumption = x.ThicknessChangeConsumption,
                ThicknessChangeTime = TimeSpan.FromMinutes(x.ThicknessChangeTimeMinutes),
                WidthChangeConsumption = x.WidthChangeConsumption,
                WidthChangeTime = TimeSpan.FromMinutes(x.WidthChangeTimeMinutes),
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
                ProductionID = productions.TryGetValue(x.ProductionName, out var production) ? production : throw new ProductionDataImportItemNotFoundException(typeof(ProductionDTO), x.ProductionName),
            }))
            {
                await _productionLineService.CreateProductionLine(dto);
            }

            var routesData = data.Routes.Select(x =>
            {
                var productionId = productions.TryGetValue(x.ProductionName, out var production) ? production : throw new ProductionDataImportItemNotFoundException(typeof(ProductionDTO), x.ProductionName);
                var customerId = customers.TryGetValue(x.CustomerName, out var customer) ? customer : throw new ProductionDataImportItemNotFoundException(typeof(CustomerDTO), x.CustomerName);

                return new { productionId, customerId, x.Price, x.DrivingTimeMinutes };
            }).GroupBy(x => x.productionId)
            .ToFrozenDictionary(x => x.Key, x => x.GroupBy(x => x.customerId).ToDictionary(x => x.Key, x => x.First()));

            var routesInfo = await _routeService.GetRoutesInfo();

            foreach (var routeInfo in routesInfo)
            {
                var route = await _routeService.GetRoute(routeInfo.ID);

                if (routesData.TryGetValue(route.ProductionInfo.EntityID, out var productionRoutes) && productionRoutes.TryGetValue(route.CustomerInfo.EntityID, out var customerRoute))
                {
                    var writeModel = new RouteWriteDTO { DrivingTime = TimeSpan.FromMinutes(customerRoute.DrivingTimeMinutes), Price = customerRoute.Price };

                    await _routeService.UpdateRoute(routeInfo.ID, writeModel);
                }
            }
            
            await _productionDataRepository.EndImport();
        }
    }
}

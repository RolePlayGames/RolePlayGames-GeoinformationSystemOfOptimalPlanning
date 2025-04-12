using GSOP.Application.Contracts.Optimization.Models;
using GSOP.Application.Contracts.Orders;
using GSOP.Application.Contracts.ProductionLines;
using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Algorithms.Contracts.Bruteforce;
using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Contracts.Optimization;
using GSOP.Domain.Contracts.Optimization.Approximation;
using GSOP.Domain.Contracts.Optimization.Bruteforce;
using GSOP.Domain.Contracts.Optimization.Genetic;
using GSOP.Domain.Contracts.Optimization.Genetic.Models;
using GSOP.Domain.Contracts.Optimization.Models;
using GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Cost;
using GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Time;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Contracts.Routes;
using GSOP.Domain.Contracts.Routes.Models;
using GSOP.Domain.Optimization.FitnessFunctionCalculators;
using GSOP.Domain.Optimization.Genetic.FitnessFunctionCalculators;
using GSOP.Domain.Optimization.Genetic.IndividualConverters;
using GSOP.Domain.Optimization.Genetic.TargetFunctionCalculators;
using GSOP.Domain.Optimization.TargetFunctionCalculators.Cost;
using GSOP.Domain.Optimization.TargetFunctionCalculators.Time;
using System.Collections.Frozen;

namespace GSOP.Application.Optimization;

public class ProductionPlanner : IProductionPlanner
{
    private static readonly string[] _originalPlanOrderNumbers = [
        "101586",
        "101585",
        "101596",
        "101597",
        "101587",
        "101589",
        "101559",
        "101543",
        "101650",
        "101590",
        "101651",
        "101648",
        "101647",
        "101646",
        "101652",
        "101663",
        "101653",
        "101613",
        "101614",
        "101632",
        "101621",
        "101615",
        "101633",
        "101662",
        "101616",
        "101627",
        "101634",
        "101654",
        "101617",
        "101635",
        "101628",
        "101626",
        "101618",
        "101636",
        "101637",
        "101619",
        "101629",
        "101638",
        "101630",
        "101639",
        "101631",
        "101640",
        "101664",
        "101641",
        "101644",
        "101642",
        "101643",
        "101645",
    ];

    private static readonly string _originalPlanProductionLineName = "MEX 08";

    private readonly IProductionLineQueueCostCalculator _productionLineQueueCostCalculator;
    private readonly IProductionLineQueueTimeCalculator _productionLineQueueTimeCalculator;
    private readonly IProductionLineFactory _productionLineFactory;
    private readonly IProductionLineService _productionLineService;
    private readonly IOrderFactory _orderFactory;
    private readonly IOrderService _orderService;
    private readonly IRouteRepository _routeRepository;

    private readonly IBruteforceDistributor _bruteforeDistributor;
    private readonly IBruteforceAlgorithmFactory _bruteforceAlgorithmFactory;

    private readonly IIndividualConverter _individualConverter;
    private readonly IGeneticAlgorithmFactory _geneticAlgorithmFactory;
    private readonly IApproximationAlgorithmFactory _approximationAlgorithmFactory;

    private readonly IOrderExcecutionTimeCalculator _orderExcecutionTimeCalculator;
    private readonly IOrdersReconfigurationTimeCalculator _ordersReconfigurationTimeCalculator;

    public ProductionPlanner(
        IProductionLineQueueCostCalculator productionLineQueueCostCalculator,
        IProductionLineQueueTimeCalculator productionLineQueueTimeCalculator,
        IProductionLineFactory productionLineFactory,
        IProductionLineService productionLineService,
        IOrderFactory orderFactory,
        IOrderService orderService,
        IBruteforceDistributor bruteforeDistributor,
        IBruteforceAlgorithmFactory bruteforceAlgorithmFactory,
        IIndividualConverter individualConverter,
        IGeneticAlgorithmFactory geneticAlgorithmFactory,
        IApproximationAlgorithmFactory approximationAlgorithmFactory,
        IOrderExcecutionTimeCalculator orderExcecutionTimeCalculator,
        IOrdersReconfigurationTimeCalculator ordersReconfigurationTimeCalculator,
        IRouteRepository routeRepository)
    {
        _productionLineQueueCostCalculator = productionLineQueueCostCalculator;
        _productionLineQueueTimeCalculator = productionLineQueueTimeCalculator;
        _productionLineFactory = productionLineFactory;
        _orderFactory = orderFactory;
        _bruteforeDistributor = bruteforeDistributor;
        _bruteforceAlgorithmFactory = bruteforceAlgorithmFactory;
        _individualConverter = individualConverter;
        _geneticAlgorithmFactory = geneticAlgorithmFactory;
        _approximationAlgorithmFactory = approximationAlgorithmFactory;
        _orderExcecutionTimeCalculator = orderExcecutionTimeCalculator;
        _ordersReconfigurationTimeCalculator = ordersReconfigurationTimeCalculator;
        _productionLineService = productionLineService;
        _orderService = orderService;
        _routeRepository = routeRepository;
    }

    public async Task<IReadOnlyCollection<ProductionPlanInfo>> CreateOptimizedProductionPlanByBruteforceAlgorithm(BruteforceAlgorithmPlanningData planningData)
    {
        ITargetFunctionCalculator<ProductionPlan> targetFunctionCalculator = planningData.FunctionType switch
        { 
            FunctionType.Cost => new CostFunctionCalculator(_productionLineQueueCostCalculator),
            _ => new TimeFunctionCalculator(_productionLineQueueTimeCalculator),
        };

        var fitnessCalculator = new MinFitnessCalculator(targetFunctionCalculator);

        var orderModels = await GetOrders(planningData.Orders);
        var productionLineModels = await GetProductionLines(planningData.ProductionLines);
        var routesMap = await GetRoutesMap(productionLineModels, orderModels);

        return _bruteforceAlgorithmFactory.CreateAlgorithm(_bruteforeDistributor, orderModels, productionLineModels, planningData.Conditions, fitnessCalculator)
            .GetResolve()
            .Select(x => ConvertProductionPlan(planningData.StartDateTime, targetFunctionCalculator, x, routesMap))
            .ToList();
    }

    public async Task<IReadOnlyCollection<ProductionPlanInfo>> CreateOptimizedProductionPlanByGeneticAlgorithm(GeneticAlgorithmPlanningData planningData)
    {
        ITargetFunctionCalculator<ProductionPlan> targetFunctionCalculator = planningData.FunctionType switch
        {
            FunctionType.Cost => new CostFunctionCalculator(_productionLineQueueCostCalculator),
            _ => new TimeFunctionCalculator(_productionLineQueueTimeCalculator),
        };

        ITargetFunctionCalculator<IIndividual<OrderPosition>> targetFunctionCalculatorProxy = planningData.FunctionType switch
        {
            FunctionType.Cost => new CostFunctionCalculatorGeneticProxy(targetFunctionCalculator, _individualConverter),
            _ => new TimeFunctionCalculatorGeneticProxy(_individualConverter, targetFunctionCalculator),
        };

        var fitnessCalculatorProxy = new MinFitnessCalculatorGeneticProxy<OrderPosition>();

        var orderModels = await GetOrders(planningData.Orders);
        var productionLineModels = await GetProductionLines(planningData.ProductionLines);
        var routesMap = await GetRoutesMap(productionLineModels, orderModels);

        return _geneticAlgorithmFactory.CreateAlgorithm(productionLineModels, orderModels, targetFunctionCalculatorProxy, fitnessCalculatorProxy, planningData.Options, planningData.Conditions, _approximationAlgorithmFactory, _orderExcecutionTimeCalculator)
            .GetResolve()
            .Select(_individualConverter.ConvertToProductionPlan)
            .Select(x => ConvertProductionPlan(planningData.StartDateTime, targetFunctionCalculator, x, routesMap))
            .ToList();
    }

    public async Task<ProductionPlanInfo> GetOriginalProductionPlan(DateTime startDateTime)
    {
        var orders = (await _orderService.GetOrdersInfo()).ToFrozenDictionary(x => x.Name, x => x.ID);
        var orderModels = await GetOrders(_originalPlanOrderNumbers.Select(x => orders.TryGetValue(x, out var orderID) ? orderID : throw new OriginalPlanOrderWasNotFoundException(x)).ToList());

        var productionLines = (await _productionLineService.GetProductionLinesInfo()).ToFrozenDictionary(x => x.Name, x => x.ID);
        var productionLineModels = await GetProductionLines([productionLines.TryGetValue(_originalPlanProductionLineName, out var lineID) ? lineID : throw new OriginalPlanProductionLineWasNotFoundException(_originalPlanProductionLineName)]);

        var routesMap = await GetRoutesMap(productionLineModels, orderModels);

        var productionPlan = new ProductionPlan { ProductionLineQueues = [new ProductionLineQueue { ProductionLine = productionLineModels.First(), Orders = orderModels }] };

        var targetFunctionCalculator = new TimeFunctionCalculator(_productionLineQueueTimeCalculator);

        return ConvertProductionPlan(startDateTime, targetFunctionCalculator, productionPlan, routesMap);
    }

    private async Task<IReadOnlyCollection<IOrder>> GetOrders(IReadOnlyCollection<long> orders)
    {
        var orderModels = new List<IOrder>(orders.Count);

        foreach (var id in orders)
        {
            orderModels.Add(await _orderFactory.Create(id));
        }

        return orderModels;
    }

    private async Task<IReadOnlyCollection<IProductionLine>> GetProductionLines(IReadOnlyCollection<long> productionLines)
    {
        var productionLineModels = new List<IProductionLine>(productionLines.Count);

        foreach (var id in productionLines)
        {
            productionLineModels.Add(await _productionLineFactory.Create(id));
        }

        return productionLineModels;
    }

    private async Task<FrozenDictionary<long, FrozenDictionary<long, RouteReadDTO>>> GetRoutesMap(IReadOnlyCollection<IProductionLine> productionLines, IReadOnlyCollection<IOrder> orders)
    {
        var productionIds = productionLines.Select(x => x.ProductionID).Distinct().ToList();
        var customerIds = orders.Select(x => x.CustomerID).Distinct().ToList();

        var routes = await _routeRepository.GetRoutesBetweenProductionsAndCustomers(productionIds, customerIds);

        return routes.GroupBy(x => x.ProductionInfo.EntityID).ToFrozenDictionary(x => x.Key, x => x.ToFrozenDictionary(x => x.CustomerInfo.EntityID));
    }

    private ProductionPlanInfo ConvertProductionPlan(DateTime startDateTime, ITargetFunctionCalculator<ProductionPlan> targetFunctionCalculator, ProductionPlan productionPlan, FrozenDictionary<long, FrozenDictionary<long, RouteReadDTO>> routesMap)
    {
        var routesQueue = routesMap.SelectMany(x => x.Value.Select(x => x.Value)).ToFrozenDictionary(x => x, x => new List<OrderRouteInfo>());

        return new()
        {
            StartDateTime = startDateTime,
            ProductionLineQueues = productionPlan.ProductionLineQueues.Select(x => ConvertProductionLineQueue(startDateTime, x, routesMap[x.ProductionLine.ProductionID], routesQueue)).ToList(),
            RoutesQueues = routesQueue.Select(x => new RoutesQueueInfo { Route = x.Key, OrderPosition = x.Value.OrderBy(x => x.OrderDeliveryStartDateTime).ToList() }).ToList(),
            TargetFunctionValue = targetFunctionCalculator.Calculate(productionPlan),
        };
    }

    private ProductionLineQueueInfo ConvertProductionLineQueue(DateTime startDateTime, ProductionLineQueue productionLineQueue, FrozenDictionary<long, RouteReadDTO> customerRoutes, FrozenDictionary<RouteReadDTO, List<OrderRouteInfo>> routesQueue)
    {
        var orderPositions = new List<OrderPositionInfo>(productionLineQueue.Orders.Count);

        var currentDateTime = startDateTime;
        currentDateTime += productionLineQueue.ProductionLine.SetupTime;

        IOrder? previousOrder = null;

        foreach (var order in productionLineQueue.Orders)
        {
            if (previousOrder is not null)
            {
                currentDateTime += TimeSpan.FromMinutes(_ordersReconfigurationTimeCalculator.Calculate(productionLineQueue.ProductionLine, previousOrder, order));
            }

            var duration = TimeSpan.FromMinutes(_orderExcecutionTimeCalculator.Calculate(order));

            var orderPosition = new OrderPositionInfo
            {
                OrderNumber = order.Number,
                OrderProductionStartDateTime = currentDateTime,
                OrderProductionEndDateTime = currentDateTime += duration,
            };

            orderPositions.Add(orderPosition);

            var route = customerRoutes[order.CustomerID];

            var orderRoute = new OrderRouteInfo()
            {
                OrderNumber = order.Number,
                OrderDeliveryStartDateTime = orderPosition.OrderProductionEndDateTime,
                OrderDeliveryEndDateTime = orderPosition.OrderProductionEndDateTime + customerRoutes[order.CustomerID].DrivingTime,
            };

            routesQueue[route].Add(orderRoute);
        }

        return new()
        {
            ProductionLineName = productionLineQueue.ProductionLine.Name,
            OrderPositions = orderPositions,
        };
    }
}

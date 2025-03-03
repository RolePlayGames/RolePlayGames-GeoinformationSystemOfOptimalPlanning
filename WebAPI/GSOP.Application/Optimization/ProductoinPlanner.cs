using GSOP.Application.Contracts.Optimization.Models;
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
using GSOP.Domain.Optimization.FitnessFunctionCalculators;
using GSOP.Domain.Optimization.Genetic.FitnessFunctionCalculators;
using GSOP.Domain.Optimization.Genetic.IndividualConverters;
using GSOP.Domain.Optimization.Genetic.TargetFunctionCalculators;
using GSOP.Domain.Optimization.TargetFunctionCalculators.Cost;
using GSOP.Domain.Optimization.TargetFunctionCalculators.Time;

namespace GSOP.Application.Optimization;

public class ProductoinPlanner : IProductionPlanner
{
    private readonly IProductionLineQueueCostCalculator _productionLineQueueCostCalculator;
    private readonly IProductionLineQueueTimeCalculator _productionLineQueueTimeCalculator;
    private readonly IProductionLineFactory _productionLineFactory;
    private readonly IOrderFactory _orderFactory;

    private readonly IBruteforceDistributor _bruteforeDistributor;
    private readonly IBruteforceAlgorithmFactory _bruteforceAlgorithmFactory;

    private readonly IIndividualConverter _individualConverter;
    private readonly IGeneticAlgorithmFactory _geneticAlgorithmFactory;
    private readonly IApproximationAlgorithmFactory _approximationAlgorithmFactory;

    private readonly IOrderExcecutionTimeCalculator _orderExcecutionTimeCalculator;
    private readonly IOrdersReconfigurationTimeCalculator _ordersReconfigurationTimeCalculator;

    public ProductoinPlanner(
        IProductionLineQueueCostCalculator productionLineQueueCostCalculator,
        IProductionLineQueueTimeCalculator productionLineQueueTimeCalculator,
        IProductionLineFactory productionLineFactory,
        IOrderFactory orderFactory,
        IBruteforceDistributor bruteforeDistributor,
        IBruteforceAlgorithmFactory bruteforceAlgorithmFactory,
        IIndividualConverter individualConverter,
        IGeneticAlgorithmFactory geneticAlgorithmFactory,
        IApproximationAlgorithmFactory approximationAlgorithmFactory,
        IOrderExcecutionTimeCalculator orderExcecutionTimeCalculator,
        IOrdersReconfigurationTimeCalculator ordersReconfigurationTimeCalculator)
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

        return _bruteforceAlgorithmFactory.CreateAlgorithm(_bruteforeDistributor, orderModels, productionLineModels, planningData.Conditions, fitnessCalculator)
            .GetResolve()
            .Select(x => ConvertProductionPlan(planningData.StartDateTime, targetFunctionCalculator, x))
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

        return _geneticAlgorithmFactory.CreateAlgorithm(productionLineModels, orderModels, targetFunctionCalculatorProxy, fitnessCalculatorProxy, planningData.Options, planningData.Conditions, _approximationAlgorithmFactory, _orderExcecutionTimeCalculator)
            .GetResolve()
            .Select(_individualConverter.ConvertToProductionPlan)
            .Concat([_approximationAlgorithmFactory.CreateAlgorithm(productionLineModels, orderModels, _orderExcecutionTimeCalculator).GetResolve()])
            .Select(x => ConvertProductionPlan(planningData.StartDateTime, targetFunctionCalculator, x))
            .ToList();
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

    private ProductionPlanInfo ConvertProductionPlan(DateTime startDateTime, ITargetFunctionCalculator<ProductionPlan> targetFunctionCalculator, ProductionPlan productionPlan)
    {
        return new()
        {
            StartDateTime = startDateTime,
            ProductionLineQueues = productionPlan.ProductionLineQueues.Select(x => ConvertProductionLineQueue(startDateTime, x)).ToList(),
            TargetFunctionValue = targetFunctionCalculator.Calculate(productionPlan),
        };
    }

    private ProductionLineQueueInfo ConvertProductionLineQueue(DateTime startDateTime, ProductionLineQueue productionLineQueue)
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

            var orderPosition = new OrderPositionInfo()
            {
                OrderNumber = order.Number,
                OrderProductionStartDateTime = currentDateTime,
                OrderProductionEndDateTime = currentDateTime += duration,
            };

            orderPositions.Add(orderPosition);
        }

        return new()
        {
            ProductionLineName = productionLineQueue.ProductionLine.Name,
            OrderPositions = orderPositions,
        };
    }
}

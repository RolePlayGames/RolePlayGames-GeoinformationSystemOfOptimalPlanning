using GSOP.Domain.Algorithms.Contracts.Genetic.Models;
using GSOP.Domain.Algorithms.Contracts;
using GSOP.Domain.Contracts.Customers;
using GSOP.Domain.Contracts.FilmRecipes;
using GSOP.Domain.Contracts.FilmTypes;
using GSOP.Domain.Contracts.Optimization;
using GSOP.Domain.Contracts.Optimization.Approximation;
using GSOP.Domain.Contracts.Optimization.Bruteforce;
using GSOP.Domain.Contracts.Optimization.Genetic;
using GSOP.Domain.Contracts.Optimization.Genetic.Models;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionData;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Customers;
using GSOP.Domain.FilmRecipes;
using GSOP.Domain.FilmTypes;
using GSOP.Domain.Optimization;
using GSOP.Domain.Optimization.Approximation;
using GSOP.Domain.Optimization.Bruteforce;
using GSOP.Domain.Optimization.Genetic;
using GSOP.Domain.Optimization.Genetic.IndividualConverters;
using GSOP.Domain.Orders;
using GSOP.Domain.ProductionData;
using GSOP.Domain.ProductionLines;
using Microsoft.Extensions.DependencyInjection;
using GSOP.Domain.Optimization.Genetic.TargetFunctionCalculators;
using GSOP.Domain.Algorithms.Contracts.Bruteforce;
using GSOP.Domain.Algorithms.Bruteforce;
using GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Cost;
using GSOP.Domain.Optimization.TargetFunctionCalculators.Cost;
using GSOP.Domain.Contracts.Optimization.TargetFunctionCalculators.Time;
using GSOP.Domain.Optimization.TargetFunctionCalculators.Time;
using GSOP.Domain.Algorithms.Bruteforce.CombinationsGenerators;
using GSOP.Domain.Optimization.TargetFunctionCalculators;
using GSOP.Domain.Productions;
using GSOP.Domain.Contracts.Productions;

namespace GSOP.Domain.DI;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add persistence for domain layer contracts
    /// </summary>
    public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddCustomerComponents()
            .AddFilmRecipeComponents()
            .AddFilmTypeComponents()
            .AddOrderComponents()
            .AddProductionComponents()
            .AddProductionLineComponents()
            .AddProductionDataComponents()
            .AddOptimizationComponents()
            ;

    internal static IServiceCollection AddCustomerComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<ICustomerFactory, CustomerFactory>();

    internal static IServiceCollection AddFilmRecipeComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IFilmRecipeFactory, FilmRecipeFactory>();

    internal static IServiceCollection AddFilmTypeComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IFilmTypeFactory, FilmTypeFactory>();

    internal static IServiceCollection AddOrderComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IOrderFactory, OrderFactory>();

    internal static IServiceCollection AddProductionComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IProductionFactory, ProductionFactory>();

    internal static IServiceCollection AddProductionLineComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IProductionLineFactory, ProductionLineFactory>();

    internal static IServiceCollection AddProductionDataComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IProductionDataImportProcessFactory, ProductionDataImportProcessFactory>();

    internal static IServiceCollection AddOptimizationComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IApproximationAlgorithmFactory, ApproximationAlgorithmFactory>()
            .AddScoped<IBruteforceAlgorithmFactory, BruteforceAlgorithmFactory>()
            .AddScoped<IBruteforceDistributor, ParallelBruteforceStackDistributor>()
            .AddScoped<ICombinationsGenerator, CombinationsGenerator>()
            .AddScoped<IExecutionTimeCalculator, ExecutionTimeCalculator>()
            .AddScoped<IFinalConditionCheckerFactory, FinalConditionCheckerFactory>()
            .AddScoped<IGeneticAlgorithmFactory, GeneticAlgorithmFactory>()
            .AddScoped<IGeneticFinalConditionCheckerFactory, GeneticFinalConditionCheckerFactory>()
            .AddScoped<IIndividualConverter, IndividualConverter>()
            .AddScoped<IOrderCostCalculator, OrderCostCalculator>()
            .AddScoped<IOrderExcecutionTimeCalculator, OrderExcecutionTimeCalculator>()
            .AddScoped<IOrdersReconfigurationCostCalculator, OrdersReconfigurationCalculator>()
            .AddScoped<IProductionLineQueueCostCalculator, ProductionLineQueueCostCalculator>()
            .AddScoped<IOrdersReconfigurationTimeCalculator, OrdersReconfigurationCalculator>()
            .AddScoped<IProductionLineQueueTimeCalculator, ProductionLineQueueTimeCalculator>()
            .AddScoped<IReconfigurationCostCalculator, ReconfigurationCostCalculator>()
            .AddScoped<IReconfigurationTimeCalculator, ReconfigurationTimeCalculator>()
        ;
}

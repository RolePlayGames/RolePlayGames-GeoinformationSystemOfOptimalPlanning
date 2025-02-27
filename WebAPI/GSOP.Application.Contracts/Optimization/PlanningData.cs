using GSOP.Domain.Contracts.Optimization.Genetic;

namespace GSOP.Domain.Contracts.Optimization;

public record PlanningData(
    DateTime StartDateTime,
    IReadOnlyCollection<long> Orders,
    IReadOnlyCollection<long> ProductionLines,
    FunctionType FunctionType);

public record BruteforceAlgorithmPlanningData(
    DateTime StartDateTime,
    IReadOnlyCollection<long> Orders,
    IReadOnlyCollection<long> ProductionLines,
    FunctionType FunctionType,
    FinalCheckerConditions Conditions) : PlanningData(
        StartDateTime,
        Orders,
        ProductionLines,
        FunctionType);

public record GeneticAlgorithmPlanningData(DateTime StartDateTime,
    IReadOnlyCollection<long> Orders,
    IReadOnlyCollection<long> ProductionLines,
    FunctionType FunctionType,
    GeneticAlgorithmOptions Options,
    GeneticFinalCheckerConditions Conditions) : PlanningData(
        StartDateTime,
        Orders,
        ProductionLines,
        FunctionType);

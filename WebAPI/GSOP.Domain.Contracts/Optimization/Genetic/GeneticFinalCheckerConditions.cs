namespace GSOP.Domain.Contracts.Optimization.Genetic;

public record GeneticFinalCheckerConditions(TimeSpan? TimeoutDelay, int? IterationsCount, int? GenerationsCount);

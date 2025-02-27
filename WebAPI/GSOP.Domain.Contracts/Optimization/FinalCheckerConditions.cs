namespace GSOP.Domain.Contracts.Optimization;

public record FinalCheckerConditions(TimeSpan? TimeoutDelay, int? IterationsCount);

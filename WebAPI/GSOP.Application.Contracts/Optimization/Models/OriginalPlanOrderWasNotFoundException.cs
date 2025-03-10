namespace GSOP.Application.Contracts.Optimization.Models;

public class OriginalPlanOrderWasNotFoundException : Exception
{
    public string OriginalOrderNumber { get; }

    public OriginalPlanOrderWasNotFoundException(string originalOrderNumber) : base($"Order from original plan was not found by number {originalOrderNumber}")
    {
        OriginalOrderNumber = originalOrderNumber;
    }
}

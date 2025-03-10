namespace GSOP.Application.Contracts.Optimization.Models;

public class OriginalPlanProductionLineWasNotFoundException : Exception
{
    public string OriginalProductionLineName { get; }

    public OriginalPlanProductionLineWasNotFoundException(string originalProductionLineName) : base($"Production line from original plan was not found by name {originalProductionLineName}")
    {
        OriginalProductionLineName = originalProductionLineName;
    }
}

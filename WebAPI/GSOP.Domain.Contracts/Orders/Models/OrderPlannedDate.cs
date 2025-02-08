namespace GSOP.Domain.Contracts.Orders.Models;

public record OrderPlannedDate
{
    private readonly DateTime? _plannedDate;

    public OrderPlannedDate(DateTime? plannedDate)
    {
        _plannedDate = plannedDate;
    }

    public static implicit operator DateTime?(OrderPlannedDate plannedDate) => plannedDate._plannedDate;

    public static explicit operator OrderPlannedDate(DateTime? plannedDate) => new(plannedDate);
}

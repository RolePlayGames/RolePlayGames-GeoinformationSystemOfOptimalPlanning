using GSOP.Domain.Contracts.Routes.Models;

namespace GSOP.Domain.Contracts.Routes;

public interface IRoute
{
    public ID ProductionID { get; }

    public ID CustomerID { get; }

    public RoutePrice Price { get; }

    public RouteDrivingTime DrivingTime { get; }

    public void SetPrice(RoutePrice price);

    public void SetDrivingTime(RouteDrivingTime time);
}

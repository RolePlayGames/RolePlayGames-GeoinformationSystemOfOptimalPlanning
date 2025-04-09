using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.Routes;
using GSOP.Domain.Contracts.Routes.Models;

namespace GSOP.Domain.Routes;

public class Route : IRoute
{
    public ID ProductionID { get; }

    public ID CustomerID { get; }

    public RoutePrice Price { get; private set; }

    public RouteDrivingTime DrivingTime { get; private set; }

    public Route(ID productionID, ID customerID, RoutePrice price, RouteDrivingTime drivingTime)
    {
        ProductionID = productionID;
        CustomerID = customerID;
        Price = price;
        DrivingTime = drivingTime;
    }

    public void SetPrice(RoutePrice price)
    {
        if (price != Price)
        {
            price = Price;
        }
    }

    public void SetDrivingTime(RouteDrivingTime time)
    {
        if (DrivingTime != time)
        {
            DrivingTime = time;
        }
    }
}

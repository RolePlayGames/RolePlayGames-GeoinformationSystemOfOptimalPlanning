using GSOP.Application.Contracts.Routes;
using GSOP.Domain.Contracts.Customers;
using GSOP.Domain.Contracts.Productions;
using GSOP.Domain.Contracts.Routes;
using GSOP.Domain.Contracts.Routes.Models;

namespace GSOP.Application.Routes;

public class RouteService : IRouteService
{
    private readonly IRouteFactory _routeFactory;
    private readonly IRouteRepository _routeRepository;
    private readonly ICustomerFactory _customerFactory;
    private readonly IProductionFactory _productionFactory;

    public RouteService(IRouteFactory routeFactory, ICustomerFactory customerFactory, IProductionFactory productionFactory, IRouteRepository routeRepository)
    {
        _routeFactory = routeFactory;
        _customerFactory = customerFactory;
        _productionFactory = productionFactory;
        _routeRepository = routeRepository;
    }

    public async Task<RouteReadDTO> GetRoute(long id)
    {
        var routeModel = await _routeFactory.Create(id);
        var customer = await _customerFactory.CreateCustomer(routeModel.CustomerID);
        var production = await _productionFactory.CreateProduction(routeModel.ProductionID);

        return new()
        {
            CustomerInfo = new()
            {
                EntityID = routeModel.CustomerID,
                EntityName = customer.Name,
                EntityCoordinates = customer.Coordinates is null
                    ? null
                    : new()
                    {
                        Latitude = customer.Coordinates.Latitude,
                        Longitude = customer.Coordinates.Longitude,
                    },
            },
            ProductionInfo = new()
            {
                EntityID = routeModel.CustomerID,
                EntityName = production.Name,
                EntityCoordinates = production.Coordinates is null
                    ? null
                    : new()
                    {
                        Latitude = production.Coordinates.Latitude,
                        Longitude = production.Coordinates.Longitude,
                    },
            },
            DrivingTime = routeModel.DrivingTime,
            Price = routeModel.Price,
        };
    }

    public Task<IReadOnlyCollection<RouteInfo>> GetRoutesInfo()
    {
        return _routeRepository.GetInfos();
    }

    public async Task UpdateRoute(long id, RouteWriteDTO route)
    {
        var routeModel = await _routeFactory.Create(id);

        routeModel.SetPrice(new(route.Price));
        routeModel.SetDrivingTime(new(route.DrivingTime));
    }
}

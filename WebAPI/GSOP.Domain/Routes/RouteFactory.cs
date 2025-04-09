using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.Customers;
using GSOP.Domain.Contracts.Orders.Models;
using GSOP.Domain.Contracts.Productions;
using GSOP.Domain.Contracts.Productions.Exceptions;
using GSOP.Domain.Contracts.Routes;
using GSOP.Domain.Contracts.Routes.Exceptions;
using GSOP.Domain.Contracts.Routes.Models;

namespace GSOP.Domain.Routes;

public class RouteFactory : IRouteFactory
{
    private readonly IRouteRepository _routeRepository;
    private readonly IProductionRepository _productionRepository;
    private readonly ICustomerRepository _customerRepository;

    public RouteFactory(IRouteRepository routeRepository, IProductionRepository productionRepository, ICustomerRepository customerRepository)
    {
        _routeRepository = routeRepository;
        _productionRepository = productionRepository;
        _customerRepository = customerRepository;
    }

    public async Task<IRoute> Create(long id)
    {
        var routeId = new ID(id);

        var route = await _routeRepository.Get(routeId) ?? throw new RouteWasNotFoundException(routeId);

        var productionId = new ID(route.ProductionInfo.EntityID);
        var customerId = new ID(route.CustomerInfo.EntityID);
        var price = new RoutePrice(route.Price);
        var drivingTime = new RouteDrivingTime(route.DrivingTime);

        return new Route(
            productionId,
            customerId,
            price,
            drivingTime);
    }

    public async Task<IReadOnlyCollection<IRoute>> CreateCustomerRoutes(ID CustomerId)
    {
        var productionInfos = await _productionRepository.GetProductionsInfo();

        var routes = new List<IRoute>(productionInfos.Count);

        foreach (var productionInfo in productionInfos)
        {
            var route = await Create(new RouteCreateDTO
            {
                CustomerID = CustomerId,
                ProductionID = productionInfo.ID,
                DrivingTime = TimeSpan.Zero,
                Price = default,
            });

            routes.Add(route);
        }

        return routes;
    }

    public async Task<IReadOnlyCollection<IRoute>> CreateProductionRoutes(ID ProductionId)
    {
        var customerInfos = await _customerRepository.GetCustomersInfo();

        var routes = new List<IRoute>(customerInfos.Count);

        foreach (var customerInfo in customerInfos)
        {
            var route = await Create(new RouteCreateDTO
            {
                CustomerID = customerInfo.ID,
                ProductionID = ProductionId,
                DrivingTime = TimeSpan.Zero,
                Price = default,
            });

            routes.Add(route);
        }

        return routes;
    }

    private async Task<IRoute> Create(RouteCreateDTO route)
    {
        var productionId = new ID(route.ProductionID);
        var isProductionExists = await _productionRepository.IsProductionExists(productionId);

        if (!isProductionExists)
            throw new ProductionDoesNotExistsException(productionId);

        var customerId = new CustomerID(route.CustomerID);
        var isCustomerExists = await _customerRepository.IsCustomerExsits(customerId);

        if (!isCustomerExists)
            throw new CustomerDoesNotExistsException(customerId);

        var price = new RoutePrice(route.Price);
        var drivingTime = new RouteDrivingTime(route.DrivingTime);

        return new Route(
            productionId,
            customerId,
            price,
            drivingTime);
    }
}

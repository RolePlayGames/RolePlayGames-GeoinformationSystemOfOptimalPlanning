using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.Locations;
using GSOP.Domain.Contracts.Routes;
using GSOP.Domain.Contracts.Routes.Models;
using LinqToDB;

namespace GSOP.Infrastructure.DataAccess.Routes;

public class RouteRepository : IRouteRepository
{
    private readonly DatabaseConnection _connection;

    public RouteRepository(DatabaseConnection connection)
    {
        _connection = connection;
    }

    /// <inheritdoc/>
    public Task<long> Create(IRoute route)
    {
        return _connection.InsertWithInt64IdentityAsync(new RoutePOCO
        {
            ProductionID = route.ProductionID,
            CustomerID = route.CustomerID,
            Price = route.Price,
            DrivingTime = route.DrivingTime,
        });
    }

    /// <inheritdoc/>
    public async Task<bool> Delete(ID id)
    {
        return await _connection.Routes
            .Where(x => x.ID == id)
            .DeleteAsync() == 1;
    }

    /// <inheritdoc/>
    public Task<RouteReadDTO?> Get(ID id)
    {
        long routeId = id;

        var query =
            from route in _connection.Routes
            where route.ID == routeId
            join production in _connection.Productions
                on route.ProductionID equals production.ID
            join customer in _connection.Customers
                on route.CustomerID equals customer.ID
            select new RouteReadDTO
            {
                ProductionInfo = new()
                {
                    EntityID = production.ID,
                    EntityName = production.Name,
                    EntityCoordinates = production.Latitude.HasValue && production.Longitude.HasValue
                        ? new CoordinatesDTO { Latitude = production.Latitude.Value, Longitude = production.Longitude.Value }
                        : null,
                },
                CustomerInfo = new()
                {
                    EntityID = customer.ID,
                    EntityName = customer.Name,
                    EntityCoordinates = customer.Latitude.HasValue && customer.Longitude.HasValue
                        ? new CoordinatesDTO { Latitude = customer.Latitude.Value, Longitude = customer.Longitude.Value }
                        : null,
                },
                Price = route.Price,
                DrivingTime = route.DrivingTime,
            };

        return query.FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<RouteInfo>> GetInfos()
    {
        var query =
            from route in _connection.Routes
            join production in _connection.Productions
                on route.ProductionID equals production.ID
            join customer in _connection.Customers
                on route.CustomerID equals customer.ID
            select new RouteInfo
            {
                ID = route.ID,
                Name = $"{production.Name} - {customer.Name}",
            };

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<RouteReadDTO>> GetRoutesBetweenProductionsAndCustomers(IReadOnlyCollection<ID> productionIds, IReadOnlyCollection<ID> customerIds)
    {
        var productionIdValues = productionIds.Select(id => (long)id).ToList();
        var customerIdValues = customerIds.Select(id => (long)id).ToList();

        var query =
            from route in _connection.Routes
            join production in _connection.Productions
                on route.ProductionID equals production.ID
            join customer in _connection.Customers
                on route.CustomerID equals customer.ID
            where productionIdValues.Contains(production.ID) && customerIdValues.Contains(customer.ID)
            select new RouteReadDTO
            {
                ProductionInfo = new()
                {
                    EntityID = production.ID,
                    EntityName = production.Name,
                    EntityCoordinates = production.Latitude.HasValue && production.Longitude.HasValue
                        ? new CoordinatesDTO { Latitude = production.Latitude.Value, Longitude = production.Longitude.Value }
                        : null,
                },
                CustomerInfo = new()
                {
                    EntityID = customer.ID,
                    EntityName = customer.Name,
                    EntityCoordinates = customer.Latitude.HasValue && customer.Longitude.HasValue
                        ? new CoordinatesDTO { Latitude = customer.Latitude.Value, Longitude = customer.Longitude.Value }
                        : null,
                },
                Price = route.Price,
                DrivingTime = route.DrivingTime,
            };

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public Task Update(ID id, IRoute route)
    {
        return _connection.Routes
            .Where(x => x.ID == id)
            .Set(x => x.Price, route.Price)
            .Set(x => x.DrivingTime, route.DrivingTime)
            .UpdateAsync();
    }
}

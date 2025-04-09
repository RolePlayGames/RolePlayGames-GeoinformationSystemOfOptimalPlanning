using GSOP.Application.Contracts.Routes;
using GSOP.Domain.Contracts.Routes.Models;
using GSOP.Interfaces.API.Productions;
using Microsoft.AspNetCore.Mvc;

namespace GSOP.Interfaces.API.Routes;

[ApiController]
[TypeFilter<ProductionsExceptionFilter>]
[Route("api/routes")]
public class RoutesController
{
    private readonly ILogger<RoutesController> _logger;
    private readonly IRouteService _routeService;

    public RoutesController(ILogger<RoutesController> logger, IRouteService routeService)
    {
        _logger = logger;
        _routeService = routeService;
    }

    [HttpGet]
    [Route("{id}")]
    public Task<RouteReadDTO> GetRoute(long id)
    {
        return _routeService.GetRoute(id);
    }

    [HttpGet]
    [Route("info")]
    public Task<IReadOnlyCollection<RouteInfo>> GetRoutesInfo()
    {
        return _routeService.GetRoutesInfo();
    }

    [HttpPost]
    [Route("{id}")]
    public Task UpdateProduction(long id, RouteWriteDTO route)
    {
        return _routeService.UpdateRoute(id, route);
    }
}

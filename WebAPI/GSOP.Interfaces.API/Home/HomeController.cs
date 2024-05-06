using GSOP.Application.Contracts.Customers;
using GSOP.Domain.Contracts.Customers.Models;
using GSOP.Interfaces.API.Customers;
using GSOP.Interfaces.API.Home.PathGetters;
using Microsoft.AspNetCore.Mvc;

namespace GSOP.Interfaces.API.Home;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    private readonly IPathGetter _pathGetter;
    private readonly IFile _file;

    public HomeController(IPathGetter pathGetter, IFile file)
    {
        _pathGetter = pathGetter;
        _file = file;
    }

    [HttpGet]
    public IActionResult GetReactStaticFileHtml()
    {
        return Content(_file.ReadAllText(_pathGetter.UiFilePath), "text/html");
    }
}
using GSOP.Application.Contracts.FilmTypes;
using GSOP.Domain.Contracts.FilmTypes.Models;
using Microsoft.AspNetCore.Mvc;

namespace GSOP.Interfaces.API.FilmTypes;

[ApiController]
[TypeFilter<FilmTypesExceptionFilter>]
[Route("api/film-types")]
public class FilmTypeController : ControllerBase
{
    private readonly ILogger<FilmTypeController> _logger;
    private readonly IFilmTypeSerivce _filmTypeService;

    public FilmTypeController(ILogger<FilmTypeController> logger, IFilmTypeSerivce filmTypeService)
    {
        _logger = logger;
        _filmTypeService = filmTypeService;
    }

    [HttpPost]
    public Task<long> CreateFilmType(FilmTypeDTO filmType)
    {
        return _filmTypeService.CreateFilmType(filmType);
    }

    [HttpDelete]
    [Route("{id}")]
    public Task DeleteFilmType(long id)
    {
        return _filmTypeService.DeleteFilmType(id);
    }

    [HttpGet]
    [Route("{id}")]
    public Task<FilmTypeDTO> GetFilmType(long id)
    {
        return _filmTypeService.GetFilmType(id);
    }

    [HttpGet]
    [Route("info")]
    public Task<IReadOnlyCollection<FilmTypeInfo>> GetCustomersInfo()
    {
        return _filmTypeService.GetFilmTypesInfo();
    }

    [HttpPost]
    [Route("{id}")]
    public Task UpdateCustomer(long id, FilmTypeDTO filmType)
    {
        return _filmTypeService.UpdateFilmType(id, filmType);
    }
}

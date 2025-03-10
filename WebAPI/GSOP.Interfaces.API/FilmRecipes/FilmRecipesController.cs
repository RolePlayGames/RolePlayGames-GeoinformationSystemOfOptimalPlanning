﻿using GSOP.Application.Contracts.FilmRecipes;
using GSOP.Domain.Contracts.FilmRecipes.Models;
using Microsoft.AspNetCore.Mvc;

namespace GSOP.Interfaces.API.FilmRecipes;

[ApiController]
[TypeFilter<FilmRecipesExceptionFilter>]
[Route("api/film-recipes")]
public class FilmRecipesController
{
    private readonly ILogger<FilmRecipesController> _logger;
    private readonly IFilmRecipeService _filmRecipeService;

    public FilmRecipesController(ILogger<FilmRecipesController> logger, IFilmRecipeService filmRecipeService)
    {
        _logger = logger;
        _filmRecipeService = filmRecipeService;
    }

    [HttpPost]
    public Task<long> CreateFilmRecipe(FilmRecipeDTO filmRecipe)
    {
        return _filmRecipeService.CreateFilmRecipe(filmRecipe);
    }

    [HttpDelete]
    [Route("{id}")]
    public Task DeleteFilmRecipe(long id)
    {
        return _filmRecipeService.DeleteFilmRecipe(id);
    }

    [HttpGet]
    [Route("avaliable-film-types")]
    public Task<IReadOnlyCollection<AvaliableFilmType>> GetAvaliableFilmTypes()
    {
        return _filmRecipeService.GetAvaliableFilmTypes();
    }

    [HttpGet]
    [Route("{id}")]
    public Task<FilmRecipeDTO> GetFilmRecipe(long id)
    {
        return _filmRecipeService.GetFilmRecipe(id);
    }

    [HttpGet]
    [Route("info")]
    public Task<IReadOnlyCollection<FilmRecipeInfo>> GetFilmRecipesInfo()
    {
        return _filmRecipeService.GetFilmRecipesInfo();
    }

    [HttpPost]
    [Route("{id}")]
    public Task UpdateFilmRecipe(long id, FilmRecipeDTO filmRecipe)
    {
        return _filmRecipeService.UpdateFilmRecipe(id, filmRecipe);
    }
}

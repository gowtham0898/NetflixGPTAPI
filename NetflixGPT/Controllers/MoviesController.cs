using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NetflixGPT.infra.DB;
using NetflixGPT.infra.DB.Entities;
using NetflixGPT.Infra.DB.Dto;
using NetflixGPT.Services.Movies;

namespace NetflixGPT.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MoviesController : ControllerBase
{
    private readonly IMoviesService _moviesService;
    public MoviesController(IMoviesService moviesService)
    {
        _moviesService = moviesService;
    }

    [HttpGet("populer")]
    public async Task<ActionResult<List<MovieEntity>>> GetPopulerMovies()
    {

        var result = await _moviesService.GetPopulerMovies();

        return Ok(result);
    }

    [HttpGet("treanding")]
    public async Task<ActionResult<List<MovieEntity>>> GetTreandingMovies()
    {
        
        var result = await _moviesService.GetTrendingMovies();

        return Ok(result);
    }
}


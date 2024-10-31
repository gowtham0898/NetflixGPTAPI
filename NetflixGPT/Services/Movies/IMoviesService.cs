using System;
using NetflixGPT.infra.DB.Entities;
using NetflixGPT.Infra.DB.Dto;

namespace NetflixGPT.Services.Movies
{
    public interface IMoviesService
    {
        Task<List<MovieEntity>> GetTrendingMovies();
        Task<List<MovieEntity>> GetPopulerMovies();
    }
}


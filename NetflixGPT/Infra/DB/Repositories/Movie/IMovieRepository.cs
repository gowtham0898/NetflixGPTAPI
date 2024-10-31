using System;
using NetflixGPT.infra.DB.Entities;

namespace NetflixGPT.Infra.DB.Repositories.Movie
{
    public interface IMovieRepository
    {
        Task<List<MovieEntity>> CreateMovies(List<MovieEntity> newMovies);

        Task<List<MovieEntity>> GetAllMovies(string category);
    }
}


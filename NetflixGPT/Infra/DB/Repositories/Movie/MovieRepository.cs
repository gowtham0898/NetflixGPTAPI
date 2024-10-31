using System;
using MongoDB.Driver;
using NetflixGPT.infra.DB;
using NetflixGPT.infra.DB.Entities;

namespace NetflixGPT.Infra.DB.Repositories.Movie
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MongoDbService _mongoDbService;
        public MovieRepository(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<List<MovieEntity>> CreateMovies(List<MovieEntity> newMovies)
        {
            try
            {
                var _movieCollection = _mongoDbService.GetMovieCollection<MovieEntity>("Movies");

                await _movieCollection.InsertManyAsync(newMovies);

                return newMovies;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<MovieEntity>> GetAllMovies(string category)
        {
            try
            {
                var movieCollection = _mongoDbService.GetMovieCollection<MovieEntity>("Movies");
                var filterBuilder = Builders<MovieEntity>.Filter;
                var filter = filterBuilder.Eq(x => x.Category, category);

                var movies = await movieCollection.Find(filter).ToListAsync();

                return movies;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


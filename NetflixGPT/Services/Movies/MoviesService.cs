using System;
using System.Text.Json;
using NetflixGPT.infra.DB.Entities;
using NetflixGPT.Infra.DB.Dto;
using NetflixGPT.Infra.DB.Repositories.Movie;
using NetflixGPT.Services.HttpClients.TraktApiService;

namespace NetflixGPT.Services.Movies
{
    public class MoviesService : IMoviesService
    {
        private readonly ITraktApiService _traktApiService;
        private readonly IMovieRepository _movieRepository;

        public MoviesService(ITraktApiService traktApiService, IMovieRepository movieRepository)
        {
            _traktApiService = traktApiService;
            _movieRepository = movieRepository;
        }

        public async Task<List<MovieEntity>> GetPopulerMovies()
        {
            List<PopulerMoviesDto> populerMovies = new List<PopulerMoviesDto>();
            var headers = new Dictionary<string, string>
                         {
                            { "X-Pagination-Page", "1" },
                            { "X-Pagination-Limit", "10" },
                            { "X-Pagination-Page-Count", "10" },
                            { "X-Pagination-Item-Count", "100" },
                            { "X-Trending-User-Count", "1721" }
                        };
            var result = await _traktApiService.GetPopularMovies(headers);
            if (result?.IsSuccessStatusCode ?? false)
            {
                var jsonresult = await result.Content.ReadAsStringAsync();

                populerMovies = JsonSerializer.Deserialize<List<PopulerMoviesDto>>(jsonresult);
            }

            var existingMovies = await _movieRepository.GetAllMovies(category: "populer");

            if(populerMovies != null && populerMovies.Any())
            {
                var newPopulerMovies = populerMovies.Where(x => !existingMovies.Any(y => x?.ids?.tmdb == y?.Ids?.Tmdb)).ToList();
                if(newPopulerMovies != null && newPopulerMovies.Any())
                {
                    List<MovieEntity> newMovies = newPopulerMovies.Select(newMovie => new MovieEntity
                    {
                        Title = newMovie.title,
                        ReleaseYear = newMovie.year,
                        Category = "populer",
                        Ids = new MovieIds
                        {
                            Trakt = newMovie.ids.trakt,
                            Slug = newMovie.ids.slug,
                            Imdb = newMovie.ids.imdb,
                            Tmdb = newMovie.ids.tmdb
                        },

                    }).ToList();
                    var createNewMovies = await _movieRepository.CreateMovies(newMovies);
                    existingMovies.AddRange(createNewMovies);
                }
            }

            return existingMovies;
        }

        public async Task<List<MovieEntity>> GetTrendingMovies()
        {
            List<TrendingMoviesDto> trendingMovies = new List<TrendingMoviesDto>();
            var headers = new Dictionary<string, string>
                         {
                            { "X-Pagination-Page", "1" },
                            { "X-Pagination-Limit", "10" },
                            { "X-Pagination-Page-Count", "10" },
                            { "X-Pagination-Item-Count", "100" },
                            { "X-Trending-User-Count", "1721" }
                        };
            var result = await _traktApiService.GetTrendingMovies(headers);

            if (result?.IsSuccessStatusCode ?? false)
            {
                var jsonresult = await result.Content.ReadAsStringAsync();

                trendingMovies = JsonSerializer.Deserialize<List<TrendingMoviesDto>>(jsonresult);

            }


            var existingMovies = await _movieRepository.GetAllMovies(category: "trending");

            if(trendingMovies != null && trendingMovies.Any())
            {
                var newTrendingMovies = trendingMovies.Where(x => !existingMovies?.Any(y => x?.movie?.ids?.tmdb == y?.Ids?.Tmdb) ?? true).ToList();

                if(newTrendingMovies != null && newTrendingMovies.Any())
                {
                    List<MovieEntity> newMovies = newTrendingMovies.Select(newMovie => new MovieEntity
                    {
                        Title = newMovie.movie.title,
                        ReleaseYear = newMovie.movie.year,
                        Category = "trending",
                        Ids = new MovieIds
                        {
                            Trakt = newMovie.movie.ids.trakt,
                            Slug = newMovie.movie.ids.slug,
                            Imdb = newMovie.movie.ids.imdb,
                            Tmdb = newMovie.movie.ids.tmdb
                        },

                    }).ToList();
                    var createNewMovies = await _movieRepository.CreateMovies(newMovies);
                    existingMovies.AddRange(createNewMovies);
                }

            }

            return existingMovies;
        }
    }
}


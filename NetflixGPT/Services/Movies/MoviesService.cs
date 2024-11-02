using System;
using System.Text.Json;
using NetflixGPT.infra.DB.Entities;
using NetflixGPT.Infra.DB.Dto;
using NetflixGPT.Infra.DB.Repositories.Movie;
using NetflixGPT.Services.HttpClients.ClientResponseModel;
using NetflixGPT.Services.HttpClients.FanArtService;
using NetflixGPT.Services.HttpClients.TraktApiService;

namespace NetflixGPT.Services.Movies
{
    public class MoviesService : IMoviesService
    {
        private readonly ITraktApiService _traktApiService;
        private readonly IMovieRepository _movieRepository;
        private readonly IFanArtService _fanArtService;
        public MoviesService(ITraktApiService traktApiService, IMovieRepository movieRepository,IFanArtService fanArtService)
        {
            _traktApiService = traktApiService;
            _movieRepository = movieRepository;
            _fanArtService = fanArtService;
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

            if (populerMovies != null && populerMovies.Any())
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

                    foreach (var eachMovie in newMovies)
                    {
                        var movieResult = await _traktApiService.GetMovieDetails(id: eachMovie.Ids.Slug);
                        if (movieResult?.IsSuccessStatusCode ?? false)
                        {
                            var movieUrl = await GetMovieTrailer(movieResult);
                            eachMovie.VideoUrl = movieUrl.Trailer;
                            eachMovie.Overview = movieUrl.Overview;
                            eachMovie.Id = null;
                        }
                    }

                    foreach (var eachMoviePhoto in newMovies)
                    {
                        var moviePhotoResult = await _fanArtService.GetMovieimages(tmdbid: eachMoviePhoto.Ids.Tmdb);

                        if (moviePhotoResult?.IsSuccessStatusCode ?? false)
                        {
                            var movieUrl = await GetMoviePosterUrl(moviePhotoResult);
                            eachMoviePhoto.PhotoUrl = movieUrl;
                            eachMoviePhoto.Id = null;
                        }
                    }

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
           
            if (trendingMovies != null && trendingMovies.Any())
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
                    foreach (var eachMovie in newMovies)
                    {
                        var movieResult = await _traktApiService.GetMovieDetails(id: eachMovie.Ids.Slug);
                        if (movieResult?.IsSuccessStatusCode ?? false)
                        {
                            var movieUrl = await GetMovieTrailer(movieResult);
                            eachMovie.VideoUrl = movieUrl.Trailer;
                            eachMovie.Overview = movieUrl.Overview;
                            eachMovie.Id = null;
                        }
                    }

                    foreach (var eachMoviePhoto in newMovies)
                    {
                        var moviePhotoResult = await _fanArtService.GetMovieimages(tmdbid: eachMoviePhoto.Ids.Tmdb);

                        if (moviePhotoResult?.IsSuccessStatusCode ?? false)
                        {
                            var movieUrl = await GetMoviePosterUrl(moviePhotoResult);
                            eachMoviePhoto.PhotoUrl = movieUrl;
                            eachMoviePhoto.Id = null;
                        }
                    }


                    var createNewMovies = await _movieRepository.CreateMovies(newMovies);
                    existingMovies.AddRange(createNewMovies);
                }

            }

            return existingMovies;
        }


        private async Task<MovieTrailerModel> GetMovieTrailer(HttpResponseMessage movieResult)
        {
            try
            {
                var movieJson = await movieResult.Content.ReadAsStringAsync();
                string trailer = null;
                string overview = null;
                // Parse the JSON to get the trailer URL only
                using (JsonDocument doc = JsonDocument.Parse(movieJson))
                {
                    // Extract the "trailer" field
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("trailer", out JsonElement trailerElement))
                    {
                        trailer =  trailerElement.GetString(); 
                    }
                    if (root.TryGetProperty("overview", out JsonElement overviewElement))
                    {
                        overview = overviewElement.GetString();
                    }
                }

                return new MovieTrailerModel
                {
                    Overview = overview,
                    Trailer = trailer
                }; 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private async Task<string> GetMoviePosterUrl(HttpResponseMessage moviePhotoResult)
        {
            try
            {
                var jsonMovieDetails = await moviePhotoResult.Content.ReadAsStringAsync();
                var movieDetails = JsonSerializer.Deserialize<MoviePosterResponse>(jsonMovieDetails);

                if(movieDetails != null && movieDetails?.movieposter != null)
                {
                    foreach (var logo in movieDetails.movieposter)
                    {
                        string logoUrl = logo.url.ToString();
                        if (!string.IsNullOrEmpty(logoUrl))
                        {
                            return logoUrl; 
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception while fetching movie logo: {ex.Message}");

            }
            return null;
        }
    }


}


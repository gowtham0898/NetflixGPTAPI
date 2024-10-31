using System;

namespace NetflixGPT.Services.HttpClients.TraktApiService
{
    public class TraktApiService : ITraktApiService
    {
        private readonly HttpClient _httpClient;

        public TraktApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://api.trakt.tv/") };
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", "47104532d058d07c0752834cd101ed23f09551a9f0504790c42d7666d6b313db");
        }

        public async Task<HttpResponseMessage> GetTrendingMovies(Dictionary<string, string> headers)
        { 
            try
            {
                //foreach (var header in headers)
                //{
                //    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                //}


                _httpClient.Timeout = TimeSpan.FromSeconds(30);
                var result = await _httpClient.GetAsync("movies/trending");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while fetching Trending movies: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception fetching Trending movies: {ex.InnerException.Message}");
                }
                return null;
            }        
        }

        public async Task<HttpResponseMessage> GetPopularMovies(Dictionary<string, string> headers)
        {
            try
            {
                //foreach (var header in headers)
                //{
                //    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                //}

                _httpClient.Timeout = TimeSpan.FromSeconds(30);
                var result = await _httpClient.GetAsync("movies/popular");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while fetching Poppuler movies: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception fetching Poppuler movies: {ex.InnerException.Message}");
                }
                return null;
            }
        }

        //https://api.trakt.tv/movies/tron-legacy-2010?extended=full   slug
    }
}


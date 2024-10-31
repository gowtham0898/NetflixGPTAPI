using System;
namespace NetflixGPT.Services.HttpClients.TraktApiService
{
    public interface ITraktApiService
    {
        Task<HttpResponseMessage> GetTrendingMovies(Dictionary<string, string> headers);
        Task<HttpResponseMessage> GetPopularMovies(Dictionary<string, string> headers);
    }
}


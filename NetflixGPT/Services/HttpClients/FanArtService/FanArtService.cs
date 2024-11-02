using System;
using System.Net.Http;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace NetflixGPT.Services.HttpClients.FanArtService
{
    public class FanArtService : IFanArtService
    {
        private readonly HttpClient _httpClient;

        public FanArtService()
        {
            _httpClient = new HttpClient(); //{ BaseAddress = new Uri("http://private-amnesiac-7a2d7b-fanarttv.apiary-proxy.com/v3/movies/293660?api_key=82075d993e038d08b5d966a683b3536d") , Timeout = TimeSpan.FromSeconds(10) };

           // _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
        }

        public async Task<HttpResponseMessage> GetMovieimages(int id)
        {
            try
            {
                //var response = await _httpClient.GetAsync($"movies/{id}?api_key=6fa42b0ef3b5f3aab6a7edaa78675ac2");
                var apiKey = "6fa42b0ef3b5f3aab6a7edaa78675ac2";

                //id.ToString();
                var response = await _httpClient.GetAsync($"http://webservice.fanart.tv/v3/movies/{id.ToString()}?api_key=82075d993e038d08b5d966a683b3536d");
                return response;

            }
            catch (Exception ex) //movieposter
            {
                Console.WriteLine($"Exception while fetching movies image url imdb: {id} "); //: {ex.Message}
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception fetching  movies image url imdb: {id} : {ex.InnerException.Message}");
                }
                return null;
            }
        }
    }
}


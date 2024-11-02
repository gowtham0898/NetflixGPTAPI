using System;
namespace NetflixGPT.Services.HttpClients.ClientResponseModel
{
    public class MoviePosterResponse
    {
        public List<MoviePoster> movieposter { get; set; }
    }

    public class MoviePoster
    {
        public string id { get; set; }
        public string url { get; set; }
       
    }
}


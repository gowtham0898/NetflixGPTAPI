using System;
namespace NetflixGPT.Services.HttpClients.FanArtService
{
    public interface IFanArtService
    {
        Task<HttpResponseMessage> GetMovieimages(int tmdbid);
    }
}


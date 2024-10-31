using System;
namespace NetflixGPT.Infra.DB.Dto
{
    public class PopulerMoviesDto
    {
        public string title { get; set; }
        public int year { get; set; }
        public PopulerMovieIdsDTO ids { get; set; }
    }
    public class PopulerMovieIdsDTO
    {
        public int trakt { get; set; }
        public string slug { get; set; }
        public string imdb { get; set; }
        public int tmdb { get; set; }
    }
}


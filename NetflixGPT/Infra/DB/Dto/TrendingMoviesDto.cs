using System;
namespace NetflixGPT.Infra.DB.Dto
{
    public class TrendingMoviesDto
    {
        public int watchers { get; set; }
        public TrendingMovieDetailsDto movie { get; set; }
    }
    public class TrendingMovieDetailsDto
    {
        public string title { get; set; }
        public int year { get; set; }
        public TrendingMovieIdsDto ids { get; set; }
    }

    public class TrendingMovieIdsDto
    {
        public int trakt { get; set; }
        public string slug { get; set; }
        public string imdb { get; set; }
        public int tmdb { get; set; }
    }
}


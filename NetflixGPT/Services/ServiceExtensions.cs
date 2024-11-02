using System;
using NetflixGPT.Authentication;
using NetflixGPT.infra.DB.Repositories.User;
using NetflixGPT.Infra.DB.Repositories.Movie;
using NetflixGPT.Services.Authentication;
using NetflixGPT.Services.HttpClients.FanArtService;
using NetflixGPT.Services.HttpClients.TraktApiService;
using NetflixGPT.Services.Movies;

namespace NetflixGPT.Services
{
	public static class ServiceExtensions
	{
		public static IServiceCollection AddServiceCollections(this IServiceCollection services)
		{
            services.AddScoped<IAuthenticationService, AuthenticationService>();

			services.AddScoped<ITraktApiService, TraktApiService>();

			services.AddScoped<IUserRepository, UserRepository>();

			services.AddScoped<IMoviesService, MoviesService>();

			services.AddScoped<IMovieRepository, MovieRepository>();
			services.AddScoped<IFanArtService, FanArtService>();
            return services;
		}
	}	
}


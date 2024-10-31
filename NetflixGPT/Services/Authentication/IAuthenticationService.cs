using System;
using NetflixGPT.infra.DB.Entities;
using NetflixGPT.Infra.DB.Dto;

namespace NetflixGPT.Services.Authentication
{
	public interface IAuthenticationService
	{
        Task<string> Authenticate(string username, string password);

        Task<UserEntity> RegisterUser(UserDto userDto);

    }
}


using System;
using Microsoft.IdentityModel.Tokens;
using NetflixGPT.infra.DB.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NetflixGPT.Services.Authentication;
using NetflixGPT.infra.DB.Repositories.User;
using Microsoft.AspNetCore.Mvc;
using NetflixGPT.Infra.DB.Dto;

namespace NetflixGPT.Authentication
{
	public class AuthenticationService  : IAuthenticationService
    {

            private readonly string _secretKey;
            private readonly IUserRepository _userRepository;

            public AuthenticationService(IConfiguration configuration, IUserRepository userRepository)
            {
                 _userRepository = userRepository;
                _secretKey = configuration["Jwt:Key"]; 
            }

            public async Task<string> Authenticate(string username, string password)
            {
              
                var user = await _userRepository.GetUser(userName: username);

                if (user == null) return null;

                // Generate JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                         new Claim(ClaimTypes.Name, user.Username),
                         new Claim(ClaimTypes.Email, user.Email)
                     }),

                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }

        public async Task<UserEntity> RegisterUser(UserDto userDto)
        {
            UserEntity userEntity = new UserEntity
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Password = userDto.Password
            };

            var newUser = await _userRepository.CreateUser(userEntity);
            return newUser;
        }
    }
}


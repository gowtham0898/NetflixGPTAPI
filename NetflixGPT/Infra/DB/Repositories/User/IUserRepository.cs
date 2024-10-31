using System;
using NetflixGPT.infra.DB.Entities;

namespace NetflixGPT.infra.DB.Repositories.User
{
    public interface IUserRepository
    {
       Task<UserEntity> GetUser(string userName);
        Task<UserEntity> CreateUser(UserEntity user);
    }
}


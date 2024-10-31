using System;
using MongoDB.Driver;
using NetflixGPT.infra.DB.Entities;

namespace NetflixGPT.infra.DB.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly MongoDbService _mongoDbService;

        public UserRepository(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<UserEntity> CreateUser(UserEntity user)
        {
            try
            {
                var userCollection = _mongoDbService.GetMovieCollection<UserEntity>("Users");

                await userCollection.InsertOneAsync(user);

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserEntity> GetUser(string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    return null;
                }

                var userCollection = _mongoDbService.GetMovieCollection<UserEntity>("Users");

                var filterBuilder = Builders<UserEntity>.Filter;
                var filter = filterBuilder.Eq(x => x.Username, userName);
                var user = await userCollection.Find(filter).FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}


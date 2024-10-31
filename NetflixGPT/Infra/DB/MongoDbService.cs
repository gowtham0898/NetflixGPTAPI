using System;
using System.Collections;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NetflixGPT.infra.DB.Entities;

namespace NetflixGPT.infra.DB
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IOptions<MongoDbSettings> settings, IMongoClient client)
        {
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetMovieCollection<T>(string collectionName)
        {

            var collection = _database.GetCollection<T>(collectionName);

            return collection;
        }
    }
}


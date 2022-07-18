using CustomerManager.Model;
using CustomerManager.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;

namespace CustomerManager.DataContexts
{
    public class MongoDBContext : IMongoDBContext
    {
        public IMongoDatabase _database { get; set; }
        private IMongoClient _client { get; set; }

        public MongoDBContext(IOptions<MongoDBSettings> dbStetting)
        {
            string connection = dbStetting.Value.ConnectionString;

            _client = new MongoClient(connection);
            _database = _client.GetDatabase(dbStetting.Value.Database);
        }

        public IMongoCollection<UserEntity> Customers { get { return _database.GetCollection<UserEntity>("CustomerEntity"); } }
    }
}
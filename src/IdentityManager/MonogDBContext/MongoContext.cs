using IdentityManager.Models;
using IdentityServer4.MongoDB;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace IdentityServer4.MongoDB.MonogDBContext
{
    public class MongoContext 
    {
        private MongoDBDatabaseSetting _requestLogging;
        private readonly ILogger _logger;

        protected IMongoClient _client;
        protected IMongoDatabase _database;

        public MongoContext(IOptions<MongoDBDatabaseSetting> requestLogging, ILoggerFactory loggerFactory)
        {
            _requestLogging = requestLogging.Value;
            _logger = loggerFactory.CreateLogger<MongoContext>();

            _client = new MongoClient(_requestLogging.ConnectionString);
            _database = _client.GetDatabase(_requestLogging.Database);
        }


        public IMongoCollection<UserEntity> UserCollection{ get { return _database.GetCollection<UserEntity>("IdentityUser"); } }        
    }
}

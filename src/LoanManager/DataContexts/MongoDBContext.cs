using CustomerManager.Model;
using LoanManager.Model;
using LoanManager.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;

namespace LoanManager.DataContexts
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

        public IMongoCollection<DecisionEntity> Decisions { get { return _database.GetCollection<DecisionEntity>("DecisionEntity"); } }
        public IMongoCollection<LoanApplicationEntity> LoanApplications { get { return _database.GetCollection<LoanApplicationEntity>("LoanApplicationEntity"); } }
        public IMongoCollection<RejectionEntity> Rejections { get { return _database.GetCollection<RejectionEntity>("RejectionEntity"); } }
    }

}
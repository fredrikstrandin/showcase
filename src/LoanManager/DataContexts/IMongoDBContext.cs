using LoanManager.Model;
using MongoDB.Driver;

namespace LoanManager.DataContexts
{
    public interface IMongoDBContext
    {
        IMongoDatabase _database { get; set; }
        IMongoCollection<DecisionEntity> Decisions { get; }
        IMongoCollection<LoanApplicationEntity> LoanApplications { get; }
        IMongoCollection<RejectionEntity> Rejections { get; }
    }
}
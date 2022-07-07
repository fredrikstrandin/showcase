using LoanManager.Interface;
using MongoDB.Bson;

namespace LoanManager.Repository
{
    public class GenerateMongoDBId : IGenerateId
    {
        public string GenerateNewId()
        {
            return ObjectId.GenerateNewId().ToString();
        }
    }
}

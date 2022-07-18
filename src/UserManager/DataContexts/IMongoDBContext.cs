using MongoDB.Driver;
using UserManager.Model;

namespace UserManager.DataContexts
{
    public interface IMongoDBContext
    {
        IMongoDatabase _database { get; set; }
        IMongoCollection<UserEntity> Users { get; }
    }
}
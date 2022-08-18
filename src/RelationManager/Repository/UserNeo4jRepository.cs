using RelationManager.Interface;
using RelationManager.Models;

namespace RelationManager.Repository
{
    public class UserNeo4jRepository : IUserRepository
    {
        private INeo4jDataAccess _neo4jDataAccess;
        private readonly ILogger<UserNeo4jRepository> _logger;

        public UserNeo4jRepository(ILogger<UserNeo4jRepository> logger, INeo4jDataAccess neo4jDataAccess)
        {
            _logger = logger;
            _neo4jDataAccess = neo4jDataAccess;
        }

        public async Task AddAsync(UserAddItem item)
        {
            try
            {
                var query = @"CREATE (n:Person {id: $id, firstname: $firstname, lastname: $lastname}) RETURN true";
                IDictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "id", item.Id},
                    { "firstname", item.FirstName },
                    { "lastname", item.LastName }
                };
                await _neo4jDataAccess.ExecuteWriteTransactionAsync<bool>(query, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }

            return;
        }
    }
}

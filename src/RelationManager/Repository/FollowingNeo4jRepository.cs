using RelationManager.Interface;
using RelationManager.Models;

namespace RelationManager.Repository
{
    public class FollowingNeo4jRepository : IFollowingRepository
    {
        private INeo4jDataAccess _neo4jDataAccess;
        private readonly ILogger<UserNeo4jRepository> _logger;

        public FollowingNeo4jRepository(ILogger<UserNeo4jRepository> logger, INeo4jDataAccess neo4jDataAccess)
        {
            _logger = logger;
            _neo4jDataAccess = neo4jDataAccess;
        }

        public async Task CreateAsync(FollowingCreateRequest reqest)
        {
            try
            {
                var query = @"MATCH (a:Person {id:$user}), (b: Person { id: $follow}) CREATE(a) -[r: FOLLOWING ]->(b) RETURN true";

                IDictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "user", reqest.UserId},
                    { "follow", reqest.FollowingId}
                };
                await _neo4jDataAccess.ExecuteWriteTransactionAsync<bool>(query, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                //throw;
            }

            return;
        }
    }
}

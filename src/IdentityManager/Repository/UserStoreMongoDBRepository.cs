using IdentityManager.Interface;
using IdentityManager.Models;
using IdentityServer4.MongoDB.MonogDBContext;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Claims;

namespace IdentityManager.Repository
{
    public class UserStoreMongoDBRepository : IUserStoreRepository
    {
        private readonly MongoContext _context;
        private readonly ILogger<UserStoreMongoDBRepository> _logger;

        public UserStoreMongoDBRepository(MongoContext context, ILogger<UserStoreMongoDBRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Finds the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public async Task<UserItem> FindBySubjectIdAsync(string subjectId)
        {
            if (ObjectId.TryParse(subjectId, out var id))
            {
                return await _context.UserCollection.Find(x => x.SubjectId == id)
                    .FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<UserItem> FindByUsernameAsync(string username)
        {
            return await _context.UserCollection.Find(x => x.Username == username)
                    .FirstOrDefaultAsync();
        }

        public async Task<UserItem> FindByExternalProviderAsync(string provider, string userId)
        {
            throw new NotImplementedException();

            //return await _context.UserCollection.Find(x =>
            //    x.ProviderName == provider &&
            //    x.ProviderSubjectId == userId)
            //    .FirstOrDefaultAsync();
        }

        public async Task<(string Hash, byte[] Salt)> FindHashPasswordAndSaltByUsernameAsync(string username)
        {
            var user = await _context.UserCollection.Find(x => x.UsernameNormalize == username.ToLower())
                .Project(x => new { x.Password, x.Salt })
                .FirstOrDefaultAsync();

            if (user != null)
            {
                return (user.Password, user.Salt);
            }

            return (null, null);
        }

        public async Task<UserItem> AddProvisionUserAsync(string name, string provider, string userId, ICollection<Claim> claims)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsActive(string subjectId)
        {
            if (ObjectId.TryParse(subjectId, out ObjectId id))
            {
                var user = await _context.UserCollection.Find(x => x.SubjectId == id)
                    .Project(x => new { x.IsActive })
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    return user.IsActive;
                }
            }

            return false;
        }

        public async Task<LoginRespons> CreateUserAsync(string nickname, string email, string hash, byte[] salt, List<Claim> claims)
        {
            var user = new UserEntity()
            {
                Email = email,
                Username = nickname,
                UsernameNormalize = nickname.ToLower(),
                Password = hash,
                Salt = salt,
                Claims = claims,
                IsActive = true
            };

            try
            {
                await _context.UserCollection.InsertOneAsync(user);

                return new LoginRespons(user.SubjectId.ToString(), null);
            }
            catch (Exception e)
            {
                _logger.LogCritical("MongoDB error: {message}", e.Message);

                return new LoginRespons(null, new MongoException("MongoDB Error"));
            }
        }
    }
}

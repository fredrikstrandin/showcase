using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManager.DataContexts;
using UserManager.Interfaces;

namespace UserManager.Repository
{
    public class KeyMongoDBRepository : IKeyRepository
    {
        private readonly IMongoDBContext _context;

        public KeyMongoDBRepository(IMongoDBContext context)
        {
            _context = context;
        }

        public IEnumerable<string> LoadEmails()
        {
            foreach (var item in (from x in _context.Users.AsQueryable()
                                  select x.Email))
            {
                yield return item;
            }

        }
    }
}

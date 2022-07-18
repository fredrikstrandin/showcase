using CustomerManager.DataContexts;
using CustomerManager.Interfaces;
using CustomerManager.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using MongoDB.Driver.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using CommonLib.Exceptions;

namespace CustomerManager.Repository
{
    public class CustomerMongoDBRepository : ICustomerRepository
    {
        private readonly ILogger<CustomerMongoDBRepository> _logger;
        private readonly IMongoDBContext _context;

        public CustomerMongoDBRepository(IMongoDBContext context, ILogger<CustomerMongoDBRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<UserItem>> GetAsync(CancellationToken cancellationToken)
        {
            var query =
                from x in _context.Customers.AsQueryable<UserEntity>()
                select new UserItem(x.Id.ToString(), x.FirstName, x.LastName, x.Email, x.Street, x.Zip, x.City);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<UserItem> GetByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (ObjectId.TryParse(userId, out ObjectId id))
            {
                var query = from x in _context.Customers.AsQueryable<UserEntity>()
                            where x.Id == id
                            select x;

                return await query.FirstOrDefaultAsync(cancellationToken);
            }
            else
            {
                return null;
            }
        }

        public async Task<CustomerRespons> UpdateAsync(UserUpdateRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updatera kund.");

            if (!ObjectId.TryParse(request.Id, out ObjectId customerId))
            {
                _logger.LogWarning("Id id not a ObjectId");

                return new CustomerRespons(request.Id, new FormatException("Id id not a ObjectId"));
            }

            FilterDefinition<UserEntity> filter = Builders<UserEntity>.Filter
                .Where(x => x.Id == customerId);

            var update = Builders<UserEntity>.Update;
            var updates = new List<UpdateDefinition<UserEntity>>();

            //Man kan inte ändra personnummer.
            //Byta lösenord bör göras i en annan process så jag lämnar det här. 

            _logger.LogInformation($"Customer {request.Id} updateras.");

            //antar att om värdet inte är ifyllt ska det inte uppdateras.

            if (!string.IsNullOrEmpty(request.FirstName))
            {
                updates.Add(update.Set(x => x.FirstName, request.FirstName));
            }

            if (!string.IsNullOrEmpty(request.LastName))
            {
                updates.Add(update.Set(x => x.LastName, request.LastName));
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                updates.Add(update.Set(x => x.Email, request.Email));
            }

            if (!string.IsNullOrEmpty(request.Street))
            {
                updates.Add(update.Set(x => x.Street, request.Street));
            }

            if (!string.IsNullOrEmpty(request.Zip))
            {
                updates.Add(update.Set(x => x.Zip, request.Zip));
            }


            if (!string.IsNullOrEmpty(request.City))
            {
                updates.Add(update.Set(x => x.City, request.City));
            }

            var ret = await _context.Customers.UpdateOneAsync(filter, update.Combine(updates));

            if (ret.ModifiedCount > 0)
            {
                _logger.LogInformation($"Kund {request.Id} skapas.");

                return new CustomerRespons(request.Id.ToString(), null);
            }
            else
            {
                return new CustomerRespons(ObjectId.Empty.ToString(), new NotFoundException($"Not found customer {request.Id}"));
            }
        }

        public async Task<CustomerRespons> CreateAsync(UserItem item, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Customer {item.Id} start created.");

            //För att kolla om användaren redan finns
            var query = from x in _context.Customers.AsQueryable<UserEntity>()
                        where x.Email == item.Email
                        select x.Email;

            try
            {
                var email = await query.FirstOrDefaultAsync(cancellationToken);

                if (email == null)
                {
                    UserEntity entity = item;
                    await _context.Customers.InsertOneAsync(entity);


                    _logger.LogInformation($"Customer {item.Id} was created.");

                    return new CustomerRespons(entity.Id.ToString(), null);
                }
                else
                {
                    _logger.LogWarning("Customer {Id} with {Email} allready exist.", item.Id, item.Email);

                    return new CustomerRespons(null, new DuplicateException($"Customer {item.Id} with {item.Email} allready exist."));
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}

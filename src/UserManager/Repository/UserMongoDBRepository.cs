using CommonLib.Exceptions;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UserManager.DataContexts;
using UserManager.Interfaces;
using UserManager.Model;

namespace UserManager.Repository;

public class UserMongoDBRepository : IUserRepository
{
    private readonly ILogger<UserMongoDBRepository> _logger;
    private readonly IMongoDBContext _context;

    public UserMongoDBRepository(IMongoDBContext context, ILogger<UserMongoDBRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<UserItem>> GetAsync()
    {
        var query =
            from x in _context.Users.AsQueryable()
            select new UserItem(x.Id.ToString(), x.FirstName, x.LastName, x.Email, x.Street, x.Zip, x.City);

        return await query.ToListAsync();
    }

    public async Task<UserItem> GetByIdAsync(string userId)
    {
        if (ObjectId.TryParse(userId, out ObjectId id))
        {
            var query = from x in _context.Users.AsQueryable()
                        where x.Id == id
                        select x;

            return await query.FirstOrDefaultAsync();
        }
        else
        {
            return null;
        }
    }

    public async Task<UserRespons> UpdateAsync(UserUpdateRequest request)
    {
        _logger.LogInformation("Updatera kund.");

        if (!ObjectId.TryParse(request.Id, out ObjectId userId))
        {
            _logger.LogWarning("Id id not a ObjectId");

            return new UserRespons(request.Id, new FormatException("Id id not a ObjectId"));
        }

        FilterDefinition<UserEntity> filter = Builders<UserEntity>.Filter
            .Where(x => x.Id == userId);

        var update = Builders<UserEntity>.Update;
        var updates = new List<UpdateDefinition<UserEntity>>();

        //Man kan inte ändra personnummer.
        //Byta lösenord bör göras i en annan process så jag lämnar det här. 

        _logger.LogInformation($"User {request.Id} updateras.");

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

        var ret = await _context.Users.UpdateOneAsync(filter, update.Combine(updates));

        if (ret.ModifiedCount > 0)
        {
            _logger.LogInformation($"Kund {request.Id} skapas.");

            return new UserRespons(request.Id.ToString(), null);
        }
        else
        {
            return new UserRespons(ObjectId.Empty.ToString(), new NotFoundException($"Not found user {request.Id}"));
        }
    }

    public async Task<UserRespons> CreateAsync(UserItem item)
    {
        _logger.LogInformation($"User {item.Id} start created.");

        //För att kolla om användaren redan finns
        var query = from x in _context.Users.AsQueryable()
                    where x.Email == item.Email
                    select x.Email;

        try
        {
            var email = await query.FirstOrDefaultAsync();

            if (email == null)
            {
                UserEntity entity = item;
                await _context.Users.InsertOneAsync(entity);


                _logger.LogInformation($"User {item.Id} was created.");

                return new UserRespons(entity.Id.ToString(), null);
            }
            else
            {
                _logger.LogWarning("User {Id} with {Email} allready exist.", item.Id, item.Email);

                return new UserRespons(null, new DuplicateException($"User {item.Id} with {item.Email} allready exist."));
            }
        }
        catch (Exception e)
        {

            throw;
        }
    }
}

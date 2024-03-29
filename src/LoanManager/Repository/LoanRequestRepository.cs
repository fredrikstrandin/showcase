﻿using CustomerManager.Model;
using LoanManager.DataContexts;
using LoanManager.Interface;
using LoanManager.Model;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Repository
{
    public class LoanRequestRepository : ILoanRequestRepository
    {
        private readonly ILogger<LoanRequestRepository> _logger;
        private readonly IMongoDBContext _context;

        public LoanRequestRepository(IMongoDBContext context, ILogger<LoanRequestRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<LoanApplicationItem>> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (ObjectId.TryParse(userId, out ObjectId id))
            {
                var query =
                    from x in _context.LoanApplications.AsQueryable<LoanApplicationEntity>()
                    where x.UserId == id
                    select new LoanApplicationItem(
                        x.Id.ToString(),
                        x.Type,
                        x.Amount,
                        x.Duration);

                return await query.ToListAsync(cancellationToken);
            }
            else
            {
                return null;
            }
        }

        public async Task<LoanApplicationItem> GetByLoanIdAsync(string userId, string loanId, CancellationToken cancellationToken)
        {
            if (ObjectId.TryParse(loanId, out ObjectId id) && ObjectId.TryParse(userId, out ObjectId uid))
            {
                var query = from x in _context.LoanApplications.AsQueryable<LoanApplicationEntity>()
                            where x.Id == id && x.UserId == uid
                            select x;

                return await query.FirstOrDefaultAsync(cancellationToken);
            }
            else
            {
                _logger.LogWarning($"LoanId {loanId} is not a ObjectId.");

                return null;
            }
        }

        public async Task<LoanRequestRespons> CreateAsync(string userId, LoanApplicationItem item, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating loan.");
            try
            {
                _logger.LogInformation($"Customer {userId} loan request is creating.");

                if (!ObjectId.TryParse(item.Id, out ObjectId id))
                {
                    _logger.LogWarning($"Id {item.Id} is not a ObjectId.");

                    return new LoanRequestRespons(null, false);
                }

                if (!ObjectId.TryParse(userId, out ObjectId uid))
                {
                    _logger.LogWarning($"UserId {userId} is not a ObjectId.");

                    return new LoanRequestRespons(null, false);
                }

                LoanApplicationEntity entity = new LoanApplicationEntity()
                {
                    Id = id,
                    UserId = uid,
                    Created = DateTime.UtcNow,
                    Amount = item.Amount ?? 0,
                    Duration = item.Duration ?? 0,
                    Type = item.Type
                };

                await _context.LoanApplications.InsertOneAsync(entity, new InsertOneOptions(), cancellationToken);

                _logger.LogInformation("Loan was created.");

                return new LoanRequestRespons(entity.Id.ToString(), true);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                return new LoanRequestRespons(null, false);
            }
        }

        public async Task<LoanRequestRespons> UpdateAsync(LoanApplicationItem item, CancellationToken cancellationToken)
        {
            //För att göra detta snabbare borde man göra en direkt Update fråga mot databasen.

            if (!ObjectId.TryParse(item.Id, out ObjectId id))
            {
                return new LoanRequestRespons(null, false);
            }

            FilterDefinition<LoanApplicationEntity> filter = Builders<LoanApplicationEntity>.Filter
                .Where(x => x.Id == id);

            var update = Builders<LoanApplicationEntity>.Update;
            var updates = new List<UpdateDefinition<LoanApplicationEntity>>();


            if (item.Amount.HasValue)
            {
                updates.Add(update.Set(x => x.Amount, item.Amount.Value));
            }

            if (item.Duration.HasValue)
            {
                updates.Add(update.Set(x => x.Duration, item.Duration.Value));
            }

            var ret = await _context.LoanApplications.UpdateOneAsync(filter, update.Combine(updates), cancellationToken: cancellationToken);

            return new LoanRequestRespons(item.Id.ToString(), ret.ModifiedCount > 0);
        }

        public async Task<LoanRequestRespons> DeleteAsync(string loanId, CancellationToken cancellationToken)
        {
            if (!ObjectId.TryParse(loanId, out ObjectId id))
            {
                return new LoanRequestRespons(null, false);
            }

            var ret =await _context.LoanApplications.DeleteOneAsync(x => x.Id == id, cancellationToken);

            return new LoanRequestRespons(loanId, ret.DeletedCount > 0);
        }
    }
}

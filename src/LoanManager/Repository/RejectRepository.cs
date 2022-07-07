using CustomerManager.Model;
using LoanManager.DataContexts;
using LoanManager.Interface;
using LoanManager.Model;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Repository
{
    public class RejectRepository : IRejectRepository
    {
        private readonly ILogger<RejectRepository> _logger;

        private readonly IMongoDBContext _context;

        public RejectRepository(IMongoDBContext context, ILogger<RejectRepository> logger)
        {
            _context = context;
            _logger = logger;
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

                RejectionEntity rejected = new RejectionEntity()
                {
                    LoanId  = id,
                    UserId = uid,
                    Created = DateTime.UtcNow,
                    Amount = item?.Amount ?? 0,
                    Duration = item?.Duration ?? 0,
                    Type = item.Type
                };

                await _context.Rejections.InsertOneAsync(rejected, cancellationToken: cancellationToken);

                _logger.LogInformation("Loan was created.");

                return new LoanRequestRespons(rejected.Id.ToString(), true);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                return new LoanRequestRespons(null, false);
            }
        }
    }
}

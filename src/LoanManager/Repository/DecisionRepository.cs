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
    public class DecisionRepository : IDecisionRepository
    {
        private readonly ILogger<DecisionRepository> _logger;
        private readonly IMongoDBContext _context;

        public DecisionRepository(IMongoDBContext context, ILogger<DecisionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(DecisionItem item, CancellationToken cancellationToken)
        {
            try
            {
                if (!ObjectId.TryParse(item.LoanId, out ObjectId id))
                {
                    return false;
                }

                if (item.Approved)
                {
                    DecisionEntity entity = new DecisionEntity()
                    {
                        LoanId = id,
                        Created = DateTime.UtcNow,
                        Aproved = true,
                        Decision = "Approved"
                    };

                    await _context.Decisions.InsertOneAsync(entity);
                }
                else
                {
                    foreach (var decision in item.Decisions)
                    {
                        DecisionEntity entity = new DecisionEntity()
                        {
                            LoanId = id,
                            Created = DateTime.UtcNow,
                            Aproved = item.Approved,
                            Decision = decision
                        };

                        await _context.Decisions.InsertOneAsync(entity);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                return false;
            }
        }
    }
}

using CustomerManager.Model;
using LoanManager.EntityFramework;
using LoanManager.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly LoanContext _context;

        public DecisionRepository(LoanContext context, ILogger<DecisionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(DecisionItem item, CancellationToken cancellationToken)
        {
            try
            {
                if (!Guid.TryParse(item.LoanId, out Guid id))
                {
                    return false;
                }

                if (item.Aproved)
                {
                    DecisionEntity entity = new DecisionEntity()
                    {
                        LoanId = id,
                        Created = DateTime.UtcNow,
                        Aproved = true,
                        Decision = "Approved"
                    };

                    _context.Decisions.Add(entity);
                }
                else
                {
                    foreach (var decision in item.Decisions)
                    {
                        DecisionEntity entity = new DecisionEntity()
                        {
                            LoanId = id,
                            Created = DateTime.UtcNow,
                            Aproved = item.Aproved,
                            Decision = decision
                        };

                        _context.Decisions.Add(entity);
                    }
                }

                int count = await _context.SaveChangesAsync(cancellationToken);

                return count > 0;
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                return false;
            }
        }
    }
}

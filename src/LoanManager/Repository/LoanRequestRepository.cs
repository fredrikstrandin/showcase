using CustomerManager.Model;
using LoanManager.EntityFramework;
using LoanManager.Interface;
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
    public class LoanRequestRepository : ILoanRequestRepository
    {
        private readonly ILogger<LoanRequestRepository> _logger;

        private readonly LoanContext _context;

        public LoanRequestRepository(LoanContext context, ILogger<LoanRequestRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<LoanApplicationItem>> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (Guid.TryParse(userId, out Guid id))
            {
                return await _context.LoanApplications
                    .Where(x => x.UserId == id)
                    .Select(x => new LoanApplicationItem()
                    {
                        Id = x.Id.ToString(),
                        UserId = x.UserId.ToString(),
                        Amount = x.Amount,
                        Duration = x.Duration,
                        Type = x.Type
                    })
                    .ToListAsync(cancellationToken);
            }
            else
            {
                return null;
            }
        }

        public async Task<LoanApplicationItem> GetByLoanIdAsync(string loanId, CancellationToken cancellationToken)
        {
            if (Guid.TryParse(loanId, out Guid id))
            {
                return await _context.LoanApplications
                        .Where(x => x.Id == id)
                        .FirstOrDefaultAsync(cancellationToken);
            }
            else
            {
                _logger.LogWarning($"LoanId {loanId} is not a Guid.");

                return null;
            }
        }

        public async Task<LoanRequestRespons> CreateAsync(LoanApplicationCreateRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating loan.");
            try
            {
                _logger.LogInformation($"Customer {request.UserId} loan request is creating.");

                if (!Guid.TryParse(request.UserId, out Guid userId))
                {
                    _logger.LogWarning($"UserId {request.UserId} is not a Guid.");

                    return new LoanRequestRespons(null, false);
                }

                LoanApplicationEntity entity = new LoanApplicationEntity()
                {
                    UserId = userId,
                    Created = DateTime.UtcNow,
                    Amount = request.Amount,
                    Duration = request.Duration,
                    Type = request.Type
                };

                _context.LoanApplications.Add(entity);

                int count = await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Loan was created.");

                return new LoanRequestRespons(entity.Id.ToString(), count > 0);
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

            if (!Guid.TryParse(item.Id, out Guid id))
            {
                return new LoanRequestRespons(null, false);
            }

            LoanApplicationEntity entity = await _context.LoanApplications.Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

            //Dessa borde ha värden så egentligen borde man göra något om deom inte har det här. 
            if (item.Amount.HasValue)
            {
                entity.Amount = item.Amount.Value;
            }

            if (item.Duration.HasValue)
            {
                entity.Duration = item.Duration.Value;
            }

            int ret = await _context.SaveChangesAsync(cancellationToken);

            return new LoanRequestRespons(entity.Id.ToString(), ret > 0);
        }

        public async Task<LoanRequestRespons> DeleteAsync(string loanId, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(loanId, out Guid id))
            {
                return new LoanRequestRespons(null, false);
            }

            _context.LoanApplications.Remove(new LoanApplicationEntity() { Id = id });

            int ret = await _context.SaveChangesAsync(cancellationToken);

            return new LoanRequestRespons(loanId, ret > 0);

        }
    }
}

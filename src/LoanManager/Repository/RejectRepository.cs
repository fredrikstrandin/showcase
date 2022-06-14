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
    public class RejectRepository : IRejectRepository
    {
        private readonly ILogger<RejectRepository> _logger;

        private readonly LoanContext _context;

        public RejectRepository(LoanContext context, ILogger<RejectRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<LoanRequestRespons> CreateAsync(LoanApplicationItem item, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating loan.");
            try
            {
                _logger.LogInformation($"Customer {item.UserId} loan request is creating.");

                if (!Guid.TryParse(item.Id, out Guid id))
                {
                    _logger.LogWarning($"Id {item.Id} is not a Guid.");

                    return new LoanRequestRespons(null, false);
                }

                if (!Guid.TryParse(item.UserId, out Guid userId))
                {
                    _logger.LogWarning($"UserId {item.UserId} is not a Guid.");

                    return new LoanRequestRespons(null, false);
                }

                RejectedEntity rejected = new RejectedEntity()
                {
                    LoanId  = id,
                    UserId = userId,
                    Created = DateTime.UtcNow,
                    Amount = item?.Amount ?? 0,
                    Duration = item?.Duration ?? 0,
                    Type = item.Type
                };

                _context.RejectedLoanApplications.Add(rejected);

                int count = await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Loan was created.");

                return new LoanRequestRespons(rejected.Id.ToString(), count > 0);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                return new LoanRequestRespons(null, false);
            }
        }
    }
}

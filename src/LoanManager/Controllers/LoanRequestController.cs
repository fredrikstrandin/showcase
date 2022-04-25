using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoanManager.Model;
using LoanManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LoanManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LoanRequestController : ControllerBase
    {
        private readonly ILogger<LoanRequestController> _logger;
        private readonly ILoanRequestService _loanRequestService;

        public LoanRequestController(ILoanRequestService loanRequestService, ILogger<LoanRequestController> logger)
        {
            _loanRequestService = loanRequestService;
            _logger = logger;
        }

        [HttpGet("userid/{userid}")]
        public async Task<List<LoanApplicationItem>> GetByUserId(string userid, CancellationToken cancellationToken)
        {
            return await _loanRequestService.GetByUserIdAsync(userid, cancellationToken);
        }

        [HttpGet("loanid/{loanid}")]
        public async Task<ActionResult<LoanApplicationItem>> GetByLoanId(string loanid, CancellationToken cancellationToken)
        {
            LoanApplicationItem item = await _loanRequestService.GetByLoanIdAsync(loanid, cancellationToken);

            if(item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<LoanApplicationRespons>> Create([FromBody] LoanApplicationCreateRequest item, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Loan request from {item.UserId} has started.");

            LoanApplicationRespons ret = await _loanRequestService.CreateAsync(item, cancellationToken);

            _logger.LogInformation($"Loan request from {item.UserId} has has ended.");

            if (ret.IsSuccess)
            {
                return Ok(ret);
            }
            else
            {
                return BadRequest(ret);
            }
        }

        [HttpPut]
        public async Task<ActionResult<LoanApplicationRespons>> Update(LoanApplicationUpdateRequest item, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Update for loan request id {item.Id} has started.");

            LoanApplicationRespons ret = await _loanRequestService.UpdateAsync(item, cancellationToken);

            _logger.LogInformation($"Update for loan request id {item.Id} has has ended.");

            if (ret.IsSuccess)
            {
                return Ok(ret);
            }
            else
            {
                return BadRequest(ret);
            }
        }

        [HttpDelete("{loanId}")]
        public async Task<ActionResult<LoanApplicationRespons>> Delete(string loanId, CancellationToken cancellationToken)
        {
            LoanApplicationRespons ret = await _loanRequestService.DeleteAsync(loanId, cancellationToken);

            if (ret.IsSuccess)
            {
                return Ok(ret);
            }
            else
            {
                return NotFound(ret);
            }
        }
    }
}

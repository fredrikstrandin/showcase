using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoanManager.Interface;
using LoanManager.Model;
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
            string userId = this.User.Claims.FirstOrDefault(i => i.Type == "sub").Value;

            LoanApplicationItem item = await _loanRequestService.GetByLoanIdAsync(userId, loanid, cancellationToken);

            if(item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<LoanApplicationRespons>> Create([FromBody] LoanApplicationCreateRequest item, CancellationToken cancellationToken)
        {
            string userId = this.User.Claims.FirstOrDefault(i => i.Type == "sub").Value;

            _logger.LogInformation($"Loan request from {userId} has started.");

            LoanApplicationRespons ret = await _loanRequestService.CreateAsync(userId, item, cancellationToken);

            _logger.LogInformation($"Loan request from {userId} has has ended.");

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
            string userId = this.User.Claims.FirstOrDefault(i => i.Type == "sub").Value;

            _logger.LogInformation($"Update for loan request id {item.Id} has started.");

            LoanApplicationRespons ret = await _loanRequestService.UpdateAsync(userId, item, cancellationToken);

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
            string role = this.User.Claims.FirstOrDefault(i => i.Type == "scope" && i.Value == "bankapi.loanadministrator")?.Value ?? String.Empty; 

            if(role != "bankapi.loanadministrator")
            {
                return Unauthorized();
            }

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

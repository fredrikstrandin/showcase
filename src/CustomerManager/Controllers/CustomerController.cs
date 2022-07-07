using CustomerManager.Model;
using CustomerManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerItem>>> Get(CancellationToken cancellationToken)
        {
            //Nyttan av att se alla utan paging går att diskutera.

            List<CustomerItem> ret = await _customerService.GetAsync(cancellationToken);

            if (ret == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(ret);
            }
        }

        [HttpGet("{userid}")]
        public async Task<ActionResult<CustomerItem>> GetByUserId(string userId, CancellationToken cancellationToken)
        {
            CustomerItem ret = await _customerService.GetByIdAsync(userId, cancellationToken);

            if (ret == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(ret);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomerRespons>> Create([FromBody] CustomerCreateRequest item, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Loan request from {item.Id} has started.");

            CustomerRespons ret = await _customerService.CreateAsync(item, cancellationToken);

            _logger.LogInformation($"Loan request from {item.Id} has has ended.");

            if (ret.Error == null)
            {
                return Ok(ret);
            }
            else
            {
                return NotFound(ret.Error.Message);
            }
        }


        [HttpPut]
        public async Task<ActionResult<CustomerRespons>> Update([FromBody] CustomerUpdateRequest request, CancellationToken cancellationToken)
        {
            //Normalt behöver man inte skicka med id, det skall vara i oauth token. Men sätter inte upp en Identity server.

            _logger.LogInformation($"Loan request from {request.Id} has started.");

            CustomerRespons ret = await _customerService.UpdateAsync(request, cancellationToken);

            _logger.LogInformation($"Loan request from {request.Id} has has ended.");

            if (ret == null)
            {
                return Ok(ret);
            }
            else
            {
                return NotFound(ret.Error.Message);
            }
        }
    }
}

using CustomerManager.Interfaces;
using CustomerManager.Model;
using IdentityModel;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerManager.Services
{
    public class CustomerService : ICustomerService
    {
        public readonly ICustomerRepository _customerRepository;
        public readonly IPasswordService _passwordService;
        public readonly IMessageService _messageService;

        public CustomerService(ICustomerRepository customerRepository, IPasswordService passwordService, IMessageService messageService)
        {
            _customerRepository = customerRepository;
            _passwordService = passwordService;
            _messageService = messageService;
        }

        public async Task<CustomerRespons> CreateAsync(CustomerCreateRequest request, CancellationToken cancellationToken)
        {
            byte[] salt = _passwordService.GenerateSalt();
            string hash = _passwordService.CreateHash(request.Password, salt);

            List<Claim> claims = new List<Claim>() { new Claim(JwtClaimTypes.Name, request.FirstName) };

            CustomerItem item = new CustomerItem(
                null,
                request.PersonalNumber,
                request.FirstName,
                request.LastName,
                request.Email,
                request.Street,
                request.Zip,
                request.City,
                request.MonthlyIncome);

            var ret = await _customerRepository.CreateAsync(item, cancellationToken);

            if (ret.IsSuccess)
            {
                await _messageService.SendLogin(request.Email, hash, salt, ret.Id, claims);
            }

            return ret;
        }

        public async Task<CustomerRespons> UpdateAsync(CustomerUpdateRequest request, CancellationToken cancellationToken)
        {
            return await _customerRepository.UpdateAsync(request, cancellationToken);
        }

        public Task<List<CustomerItem>> GetAsync(CancellationToken cancellationToken)
        {
            return _customerRepository.GetAsync(cancellationToken);
        }

        public async Task<CustomerItem> GetByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _customerRepository.GetByIdAsync(userId, cancellationToken);
        }
    }
}

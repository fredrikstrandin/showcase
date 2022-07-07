using CustomerManager.Interfaces;
using CustomerManager.Model;
using IdentityModel;
using MongoDB.Bson;
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
        
        public CustomerService(ICustomerRepository customerRepository, IPasswordService passwordService)
        {
            _customerRepository = customerRepository;
            _passwordService = passwordService;
        }

        public async Task<CustomerRespons> CreateAsync(CustomerCreateRequest request, CancellationToken cancellationToken)
        {
            List<Claim> claims = new List<Claim>() { new Claim(JwtClaimTypes.Name, request.FirstName) };

            CustomerItem item = new CustomerItem(
                request.Id,
                request.FirstName,
                request.LastName,
                request.Email,
                request.Street,
                request.Zip,
                request.City,
                request.MonthlyIncome);

            var ret = await _customerRepository.CreateAsync(item, cancellationToken);

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

        public async Task<int> GetMonthlyIncomeAsync(string Id, CancellationToken cancellationToken)
        {
            return await _customerRepository.GetMonthlyIncomeAsync(Id, cancellationToken);
        }
    }
}

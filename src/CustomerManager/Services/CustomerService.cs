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

            UserItem item = new UserItem(
                request.Id,
                request.FirstName,
                request.LastName,
                request.Email,
                request.Street,
                request.Zip,
                request.City);

            var ret = await _customerRepository.CreateAsync(item, cancellationToken);

            return ret;
        }

        public async Task<CustomerRespons> UpdateAsync(UserUpdateRequest request, CancellationToken cancellationToken)
        {
            return await _customerRepository.UpdateAsync(request, cancellationToken);
        }

        public Task<List<UserItem>> GetAsync(CancellationToken cancellationToken)
        {
            return _customerRepository.GetAsync(cancellationToken);
        }

        public async Task<UserItem> GetByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _customerRepository.GetByIdAsync(userId, cancellationToken);
        }
    }
}

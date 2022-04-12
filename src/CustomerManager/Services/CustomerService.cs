using CustomerManager.Model;
using CustomerManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
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
            byte[] salt = _passwordService.GenerateSalt();
            string hash = _passwordService.CreateHash(request.Password, salt);

            CustomerCreateItem item = new CustomerCreateItem(request.PersonalNumber, request.FirstName, request.LastName, request.Email, hash, salt, request.Street, request.Zip, request.City, request.MonthlyIncome);
            
            return await _customerRepository.CreateAsync(item, cancellationToken);
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

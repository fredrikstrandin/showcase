using CustomerManager.EntityFramework;
using CustomerManager.Interfaces;
using CustomerManager.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerManager.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ILogger<CustomerRepository> _logger;

        private readonly CustomerContext _context;

        public CustomerRepository(CustomerContext context, ILogger<CustomerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<CustomerItem>> GetAsync(CancellationToken cancellationToken)
        {
            return await _context.Customers
                .Select(x => new CustomerItem(x.Id.ToString(), x.PersonalNumber, x.FirstName, x.LastName, x.Email, x.Street, x.Zip, x.City, x.MonthlyIncome))
                .ToListAsync(cancellationToken);
        }

        public async Task<CustomerItem> GetByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (Guid.TryParse(userId, out Guid id))
            {
                return await _context.Customers
                        .Where(x => x.Id == id)
                        .FirstOrDefaultAsync(cancellationToken);
            }
            else
            {
                return null;
            }
        }

        public async Task<int> GetMonthlyIncomeAsync(string id, CancellationToken cancellationToken)
        {
            if (Guid.TryParse(id, out Guid customerId))
            {
                return await _context.Customers
                        .Where(x => x.Id == customerId)
                        .Select(x => x.MonthlyIncome)
                        .FirstOrDefaultAsync(cancellationToken);
            }
            else
            {
                return 0;
            }
        }
        public async Task<CustomerRespons> UpdateAsync(CustomerUpdateRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updatera kund.");
            try
            {
                if (!Guid.TryParse(request.Id, out Guid customerId))
                {
                    return new CustomerRespons(request.Id, false);
                }

                //För att kolla om användaren redan finns
                CustomerEntity exist = await _context.Customers
                    .Where(x => x.Id == customerId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (exist == null)
                {
                    _logger.LogInformation($"Kund {request.Id} skapas.");

                    return new CustomerRespons(request.Id, false);
                }
                else
                {
                    //Man kan inte ändra personnummer.
                    //Byta lösenord bör göras i en annan process så jag lämnar det här. 

                    _logger.LogInformation($"Customer {request.Id} updateras.");

                    //antar att om värdet inte är ifyllt ska det inte uppdateras.

                    if (!string.IsNullOrEmpty(request.FirstName) && exist.FirstName != request.FirstName)
                    {
                        exist.FirstName = request.FirstName;
                    }

                    if (!string.IsNullOrEmpty(request.LastName) && exist.LastName != request.LastName)
                    {
                        exist.LastName = request.LastName;
                    }

                    if (!string.IsNullOrEmpty(request.Email) && exist.Email != request.Email)
                    {
                        exist.Email = request.Email;
                    }

                    if (!string.IsNullOrEmpty(request.Street) && exist.Street != request.Street)
                    {
                        exist.Street = request.Street;
                    }

                    if (!string.IsNullOrEmpty(request.Zip) && exist.Zip != request.Zip)
                    {
                        exist.Zip = request.Zip;
                    }


                    if (!string.IsNullOrEmpty(request.City) && exist.City != request.City)
                    {
                        exist.City = request.City;
                    }

                    //Borde vara int? så null är att man inte updaterar
                    if (request.MonthlyIncome.HasValue && exist.MonthlyIncome != request.MonthlyIncome)
                    {
                        exist.MonthlyIncome = request.MonthlyIncome.Value;
                    }

                    await _context.SaveChangesAsync(cancellationToken);

                    return new CustomerRespons(exist.Id.ToString(), true);
                }
            }
            catch (Exception)
            {
                return new CustomerRespons(Guid.Empty.ToString(), false);
            }
        }
        public async Task<CustomerRespons> CreateAsync(CustomerCreateItem item, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Customer {item.PersonalNumber} start created.");

                //För att kolla om användaren redan finns
                CustomerEntity exist = await _context.Customers
                    .Where(x => x.PersonalNumber == item.PersonalNumber)
                    .FirstOrDefaultAsync(cancellationToken);

                if (exist == null)
                {
                    exist = item;

                    _context.Customers.Add(exist);

                    await _context.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation($"Customer {item.PersonalNumber} was created.");

                    return new CustomerRespons(exist.Id.ToString(), true);
                }
                else
                {

                    _logger.LogWarning($"Customer {item.PersonalNumber} was not created.");

                    return new CustomerRespons(exist.Id.ToString(), false);
                }
            }
            catch (Exception)
            {
                return new CustomerRespons(Guid.Empty.ToString(), false);
            }
        }
    }
}

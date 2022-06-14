using LoanManager.Interface;
using LoanManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Validation
{
    public class MonthlyIncomeValidation : ILoanApplicationValidation
    {
        private readonly ICustomerRepository _customerRepository;
        
        public MonthlyIncomeValidation(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public bool IsLoanToValidation(LoanType type)
        {
            return type == LoanType.PrivateLoan;
        }

        public async Task<LoanValidationRespons> Validation(LoanApplicationItem item, CancellationToken cancellationToken)
        {
            int MonthlyIncome = await _customerRepository.GetMonthlyIncomeAsync(item.UserId, cancellationToken);

            if (MonthlyIncome <= 31000)
            {
                return new LoanValidationRespons(false, "The monthly salary is too low.");
            }
            else
            {
                return new LoanValidationRespons(true);
            }
        }
    }
}

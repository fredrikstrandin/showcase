using LoanManager.Interface;
using LoanManager.Model;
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

        public async Task<LoanValidationRespons> Validation(LoanApplicationCreateItem item, CancellationToken cancellationToken)
        {
            int? MonthlyIncome = await _customerRepository.GetMonthlyIncomeAsync(item.UserId, cancellationToken);

            if (MonthlyIncome == null)
            {
                return new LoanValidationRespons(false, "The monthly salary is unknow.");
            }

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

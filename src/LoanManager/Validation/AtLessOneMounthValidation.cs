using LoanManager.Interface;
using LoanManager.Model;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Validation
{
    public class AtLessOneMounthValidation : ILoanApplicationValidation
    {
        public bool IsLoanToValidation(LoanType type)
        {
            return type == LoanType.PrivateLoan || type ==LoanType.CompanyLoan;
        }

        public Task<LoanValidationRespons> Validation(LoanApplicationCreateItem request, CancellationToken cancellationToken)
        {
            if (request.Duration.HasValue && request.Duration.Value > 0)
            {
                return Task.FromResult(new LoanValidationRespons(true));
            }
            else
            {
                return Task.FromResult(new LoanValidationRespons(false, "Loan duration is not over 1 mounth"));
            }

        }
    }
}

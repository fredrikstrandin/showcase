using LoanManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Interface
{
    public interface ILoanApplicationValidation
    {
        //Metoden kollar vilken eller vilak lån som skall valideras.
        bool IsLoanToValidation(LoanType type);
        Task<LoanValidationRespons> Validation(LoanApplicationCreateItem request, CancellationToken cancellationToken);
    }
}

using LoanManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Validation
{
    public interface ILoanApplicationValidation
    {
        //Metoden kollar vilken eller vilak lån som skall valideras.
        bool IsLoanToValidation(LoanType type);        
        Task<LoanValidationRespons> Validation(LoanApplicationItem request, CancellationToken cancellationToken);
    }
}

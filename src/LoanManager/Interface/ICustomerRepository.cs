using LoanManager.Model;
using System.Threading;
using System.Threading.Tasks;

namespace LoanManager.Interface;

public interface ICustomerRepository
{
    //return borde vara mer än lönen. Tex om det gick bra eller inte. 
    Task<int> GetMonthlyIncomeAsync(string id, CancellationToken cancellationToken);
}

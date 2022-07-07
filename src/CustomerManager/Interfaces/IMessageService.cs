using CustomerManager.Model;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CustomerManager.Services
{
    public interface IMessageService
    {
        Task SendNewUserAsync(CustomerCreateRequest request);
    }
}
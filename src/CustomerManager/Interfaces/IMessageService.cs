using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CustomerManager.Services
{
    public interface IMessageService
    {
        Task SendLogin(string nickname, string password, byte[] salt, List<Claim> claims);
    }
}
using System.Threading.Tasks;
using UserManager.Model;

namespace UserManager.Interfaces
{
    public interface IMessageService
    {
        Task SendNewUserAsync(UserCreateRequest request);
    }
}
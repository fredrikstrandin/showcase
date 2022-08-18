using UserManager.Model;

namespace SampleManager.Interface
{
    public interface ISampleService
    {
        Task<UsersRespons> CreateUsersAsync(int count);
    }
}

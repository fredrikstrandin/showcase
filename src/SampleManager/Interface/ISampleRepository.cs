using UserManager.Model;

namespace SampleManager.Interface
{
    public interface ISampleRepository
    {
        Task<UsersRespons> CreateUsersAsync(int count);
    }
}

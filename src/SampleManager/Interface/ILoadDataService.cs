using GenerateData.Models;

namespace SampleManager.Interface
{
    public interface ILoadDataService
    {
        Task<UserItem> GetUser();
        Task<StreetItem> GetStreet();
        Task<NameItem> GetName();
        Task<string> GetDoman();

    }
}

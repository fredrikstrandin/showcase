using CommonLib.Models;

namespace NorthStarGraphQL.Interface
{
    public interface ISampleRepository
    {
        Task<ErrorItem> CreateSampleUserAsync(int count);
    }
}

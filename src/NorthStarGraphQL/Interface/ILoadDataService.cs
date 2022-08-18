namespace NorthStarGraphQL.Interface
{
    public interface ILoadDataService
    {
        Task<StreetItem> GetStreet();
        Task<NameItem> GetFirstname();
        Task<string> Getlastname();
        Task<string> GetDoman();

    }
}

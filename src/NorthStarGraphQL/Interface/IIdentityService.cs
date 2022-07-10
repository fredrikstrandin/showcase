using NorthStarGraphQL.Models;

namespace NorthStarGraphQL.Interface;

public interface IIdentityService
{
    void AddNewUserMessage(UserCreateItem newUser);
    IObservable<UserCreateItem> GetNewUser();

    Task<(string id, string error)> CreateLoginAsync(LoginCreateItem item);
}
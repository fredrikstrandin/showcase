using NorthStarGraphQL.Interface;
using NorthStarGraphQL.Models;
using System.Reactive.Subjects;

namespace NorthStarGraphQL.Services;

public class IdentityService : IIdentityService
{
    private readonly IIdentityRepository _messageRepository;
    private readonly ISubject<UserCreateItem> _userStream = new ReplaySubject<UserCreateItem>(10);
    public IdentityService(IIdentityRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public void AddNewUserMessage(UserCreateItem newUser)
    {
        if(newUser != null)
        {
            _userStream.OnNext(newUser);
        }
    }

    public IObservable<UserCreateItem> GetNewUser()
    {
        return _userStream;
    }
    public Task<(string id, string error)> CreateLoginAsync(LoginCreateItem item)
    {        
        return _messageRepository.CreateLoginAsync(item);
    }
}
using Google.Protobuf;
using Northstar.Message;
using NorthStarGraphQL.Interface;
using NorthStarGraphQL.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NorthStarGraphQL.Services;

public class IdentityService : IIdentityService
{
    private readonly IIdentityRepository _messageRepository;

    public IdentityService(IIdentityRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public Task<(string id, string error)> CreateLoginAsync(LoginCreateItem item)
    {        
        return _messageRepository.CreateLoginAsync(item);
    }
}
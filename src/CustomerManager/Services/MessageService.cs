using CustomerManager.Interfaces;
using Google.Protobuf;
using MongoDB.Bson;
using Showcase.Message;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CustomerManager.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;

    public MessageService(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public Task SendLogin(string nickname, string password, byte[] salt, string sub, List<Claim> claims)
    {
        IdentityMessage message = new IdentityMessage()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Login = new Login()
            {
                Nickname = nickname,
                Password = password,
                Salt = ByteString.CopyFrom(salt),
                Sub = sub
            }
        };

        foreach (var item in claims)
        {
            message.Login.Claims.Add(new Login.Types.Claim() { Type = item.Type, Value = item.Value });
        }

        return _messageRepository.SendMessageAsync(ObjectId.GenerateNewId().ToString(), message, "identity");
    }
}
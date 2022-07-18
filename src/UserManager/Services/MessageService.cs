using CommonLib.Settings;
using Microsoft.Extensions.Options;
using Northstar.Message;
using System.Threading.Tasks;
using UserManager.Interfaces;
using UserManager.Model;

namespace UserManager.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly KafkaSetting _kafkaSettings;

    public MessageService(IMessageRepository messageRepository, IOptions<KafkaSetting> kafkaSettings)
    {
        _messageRepository = messageRepository;
        _kafkaSettings = kafkaSettings.Value;
    }

    public Task SendNewUserAsync(UserCreateRequest request)
    {
        NewUserMessage newUser = new NewUserMessage()
        {
            Id = request.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Street = request.Street,
            Zip = request.Zip,
            City = request.City
        };

        UserKafkaMessage message = new UserKafkaMessage() { NewUserMessage = newUser };

        return _messageRepository.SendMessageAsync(request.Id, message, _kafkaSettings.Topics["usermanager"]);
    }
}